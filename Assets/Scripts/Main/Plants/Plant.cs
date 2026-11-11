using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlantHealth))]
[RequireComponent(typeof(PlantEmotions))]
[RequireComponent(typeof(PlantsAnimator))]
[RequireComponent(typeof(PlantMover))]
[RequireComponent(typeof(PlantsRagdoll))]
[RequireComponent(typeof(PlantSensor))]
public class Plant : PhysicObject
{
    public string PlantId;
    public bool IsSleeping = false;
    public System.Action OnExcavation = delegate { };
    public System.Action OnDeath = delegate { };
    public float Progress => _stageProgress / _currentConfig.Duration;
    public PlantStageConfig CurrentConfig => _currentConfig;
    public PlantHealth Health => _plantHealth;
    public PlantsAnimator Animator => _plantAnimator;
    public PlantMover Mover => _plantMover;
    public PlantsRagdoll Ragdoll => _plantsRagdoll;
    public PlantEmotions Emotion => _plantEmotions;
    public PlantSensor Sensor => _plantSensor;

    [SerializeField] private List<OnChangeStageEvent> _stageChangeEvents = new();
    [SerializeField] private List<PlantStageConfig> _stageConfigs = new();
    [SerializeField] private List<PlantRaiseMultiplier> _dirtBonus = new();
    [SerializeField] private Seed _prefab;

    [SerializeField] private AudioSource _onSlapSound;

    [SerializeField] private AnimationCurve _seedsEvaluation;

    [SerializeField] private float _defaultScale = .3f;
    [SerializeField] private bool _isTick;

    private PlantHealth _plantHealth;
    private PlantEmotions _plantEmotions;
    private PlantsAnimator _plantAnimator;
    private PlantMover _plantMover;
    private PlantsRagdoll _plantsRagdoll;
    private PlantSensor _plantSensor;
    private PlantStageConfig _currentConfig;
    private PlantRaiseMultiplier _currentBonus;

    private float _stageProgress;
    private Coroutine _slapCoroutine;
    private Vector3 _currentScale;

    private bool _isViolatedDeath;

    private void Start()
    {
        _currentScale = Vector3.one * _defaultScale;
        var timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        if (timeManager != null)
            timeManager.OnDayChange += Grow;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        _canGrab = false;
        _stageConfigs = _stageConfigs.OrderBy(x => x.Stage).ToList();

        _plantHealth = GetComponent<PlantHealth>();
        _plantEmotions = GetComponent<PlantEmotions>();
        _plantAnimator = GetComponent<PlantsAnimator>();
        _plantMover = GetComponent<PlantMover>();
        _plantsRagdoll = GetComponent<PlantsRagdoll>();
        _plantSensor = GetComponent<PlantSensor>();

        SetBonus(PlantType.Default);

        _plantsRagdoll.Initialize(this);
        _plantSensor.Initialize(this);

        if (_currentConfig == null)
        {
            _currentConfig = _stageConfigs[0];
        }

        _plantHealth.OnDeath += HandleDeath;

        OnDeath += HandleDeath;
        OnExcavation += HandleExcavation;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _currentScale, Time.deltaTime);

        if (!_isTick) return;

        if (_plantHealth.IsAlive && !_canGrab)
            _plantHealth.Tick(Time.deltaTime, _currentConfig);

        if (_plantHealth.IsAlive)
            _plantEmotions.Tick(Time.deltaTime, _plantHealth);
    }

    public override void Grab(float force, CameraHandler handler, out float speedScale, HandController hand)
    {
        speedScale = 1f;
        if (!_canMove) return;
        base.Grab(force, handler, out speedScale, hand);
    }

    public override void UnGrab(HandController hand, out float speedScale)
    {
        speedScale = 1f;
        if (!_canMove) return;
        base.UnGrab(hand, out speedScale);
    }

    public void Kill()
    {
        _isViolatedDeath = true;
        _plantHealth.ModifyHealth(-100);
    }

    public void ApplyDamage(float damage)
    {
        _plantHealth.ModifyHealth(-damage);
    }

    public void ApplyImmunityDamage(float damage)
    {
        _plantHealth.ModifyImmunity(-damage);
    }

    public void ApplyHealth(float value)
    {
        if(_plantHealth.IsAlive)
            _plantHealth.ModifyHealth(value);
    }

    public void ApplyImmunity(float value)
    {
        _plantHealth.ModifyImmunity(value);
    }

    public void SetBonus(PlantType plantType)
    {
        if (plantType == PlantType.Default)
        {
            _currentBonus = new PlantRaiseMultiplier
            {
                Bonus = 0f
            };
            return;
        }

        _currentBonus = _dirtBonus.FirstOrDefault(x => x.PlantType == plantType);
    }

    public void AllowMove()
    {
        _plantMover.Enable();
    }

    public void Slap(RagdollPart part, float hitForce)
    {
        if (_plantHealth.IsAlive) _onSlapSound.Play();
        if (_slapCoroutine != null)
            StopCoroutine(_slapCoroutine);

        _slapCoroutine = StartCoroutine(SlapEnumerator());
    }

    public void TryEffort(ToolItemObject itemObject)
    {
        foreach (var effort in itemObject.Efforts)
        {
            _plantHealth.ModifyAttribute(effort.Name, effort.GetValue());
        }
    }

    private void Grow()
    {
        if (!_plantHealth.IsAlive) return;

        _stageProgress += 1 + _currentBonus!.Bonus;
        if(_currentConfig.Stage == PlantStage.Seed)
        {
            _currentScale = Vector3.one * (_stageProgress / _currentConfig.Duration);
        }
        if (_stageProgress >= _currentConfig.Duration)
        {
            UpdateNextStage();
        }
    }

    public void UpdateNextStage()
    {
        _currentScale = Vector3.one;
        var index = _stageConfigs.IndexOf(_currentConfig) + 1;
        if (index >= _stageConfigs.Count)
        {
            OnDeath?.Invoke();
        }

        var newStage = _stageConfigs[index];
        _currentConfig = newStage;
        _stageProgress = 0f;

        HandleStageChange();
    }

    private void HandleDeath()
    {
        if (_isViolatedDeath)
        {
            _plantSensor.SignalizeOnViolatedDeath();
        }
        else
        {
            _plantSensor.SignalizeOnPhysicDeath();
        }

        _plantsRagdoll.EnableRagdoll();
        _plantAnimator.DisableAnimator();
        _plantMover.Disable();


        var fertility = _plantHealth.PlantAttributes.FirstOrDefault(x => x.Id == "Fertility");
        var seedsCount = 5;

        if (fertility != null)
        {
            seedsCount = Mathf.RoundToInt(_seedsEvaluation.Evaluate(fertility.CurrentValue));
        }

        for (int i = 0; i < seedsCount; i++)
        {
            var position = transform.position + Random.insideUnitSphere;
            if (Physics.CheckSphere(position, 1f))
            {
                var instance = Instantiate(_prefab);
                instance.InstanceId = Guid.NewGuid().ToString();
                instance.Heritables = new Heritables
                {
                    MaxImmunity = _plantHealth.MaxImmunity,
                    MaxHealth = _plantHealth.MaxHealth,
                };
                instance.transform.position = position;
            }
        }
    }

    private void HandleStageChange()
    {
        var onChangeStage = _stageChangeEvents.FirstOrDefault(x => x.RequiredStage == _currentConfig.Stage);
        if (onChangeStage != null)
        {
            onChangeStage.Invoke(this);
        }
    }

    private void HandleExcavation()
    {
        _canGrab = true;
        _canMove = true;
        _rb.useGravity = true;
        _rb.interpolation = RigidbodyInterpolation.Extrapolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rb.isKinematic = false;
    }

    private IEnumerator SlapEnumerator()
    {
        _plantsRagdoll.EnableRagdoll();
        _plantAnimator.DisableAnimator();

        if (!_plantHealth.IsAlive) yield break;

        yield return new WaitForSeconds(.3f);

        _plantsRagdoll.DisableRagdoll();
        _plantAnimator.EnableAnimator();
    }
}

[System.Serializable]
public class PlantRaiseMultiplier
{
    public PlantType PlantType;
    public float Bonus = 100f;
}

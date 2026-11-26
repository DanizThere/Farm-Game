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
public class Plant : NPC
{
    public string PlantId;
    public bool IsSleeping = false;
    public System.Action OnExcavation = delegate { };
    public System.Action OnDeath = delegate { };
    public float Progress => _stageProgress / _currentConfig.Duration;
    public PlantStageConfig CurrentConfig => _currentConfig;
    public PlantHealth Health => _plantHealth;
    public PlantsAnimator Animator => _plantAnimator;
    public PlantsRagdoll Ragdoll => _plantsRagdoll;
    public PlantEmotions Emotion => _plantEmotions;
    public PlantSensor PlantSensor => _plantSensor;
    public Dirt Dirt => _dirt;

    [SerializeField] private Transform _modelTransform;

    [SerializeField] private List<OnChangeStageEvent> _stageChangeEvents = new();
    [SerializeField] private List<PlantStageConfig> _stageConfigs = new();
    [SerializeField] private PlantRaiseData _plantRaiseData;
    [SerializeField] private Seed _prefab;
    [SerializeField] private Fetus _fetus;

    [SerializeField] private AudioSource _onSlapSound;

    [SerializeField] private PlantStage _startStage = PlantStage.Seed;
    [SerializeField] private float _mutliplyingChance = .5f; 
    [SerializeField] private float _defaultScale = .3f;
    [SerializeField] private bool _isTick;
    [SerializeField] private PlantStage _requiredStageToBehaviouring = PlantStage.Vegetative;
    [SerializeField] private Dirt _dirt;


    private PlantHealth _plantHealth;
    private PlantEmotions _plantEmotions;
    private PlantsAnimator _plantAnimator;
    private PlantsRagdoll _plantsRagdoll;
    private PlantSensor _plantSensor;
    private PlantStageConfig _currentConfig;
    private PlantRaiseMultiplier _currentBonus;

    private float _stageProgress;
    private Coroutine _slapCoroutine;
    private Vector3 _currentScale;

    private bool _isViolatedDeath;

    private void Update()
    {
        _modelTransform.localScale = Vector3.Lerp(_modelTransform.localScale, _currentScale, Time.deltaTime);

        if (!_plantHealth.IsAlive) return;

        if (_currentConfig.Stage >= _requiredStageToBehaviouring)
        {
            NPCUpdate();
        }
        
        if (!_isTick) return;

        _plantHealth.Tick(Time.deltaTime, _currentConfig);
        _plantEmotions.Tick(Time.deltaTime, _plantHealth);
    }

    protected override void NPCStart()
    {
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        _timeManager.OnDayChange += Grow;

        SearchDirt();

        _currentScale = Vector3.one * _defaultScale;
        _modelTransform.localScale = _currentScale;

        if (_currentConfig.Stage < PlantStage.Vegetative) return;
        InitializeActivity();
    }

    protected override void NPCAwake()
    {
        base.NPCAwake();
        _stageConfigs = _stageConfigs.OrderBy(x => x.Stage).ToList();

        _plantHealth = GetComponent<PlantHealth>();
        _plantEmotions = GetComponent<PlantEmotions>();
        _plantAnimator = GetComponent<PlantsAnimator>();
        _plantsRagdoll = GetComponent<PlantsRagdoll>();
        _plantSensor = GetComponent<PlantSensor>();

        SetBonus(DirtType.Default);

        _plantsRagdoll.Initialize(this);
        _plantSensor.Initialize(this);

        if (_currentConfig == null)
        {
            var startStage = _stageConfigs.FirstOrDefault(x => x.Stage == _startStage);
            if(startStage == null)
            {
                _currentConfig = _stageConfigs[0];
                return;
            }
            _currentConfig = startStage;
        }

        _plantHealth.OnDeath += HandleDeath;

        OnDeath += HandleDeath;
        OnExcavation += HandleExcavation;
    }

    public void SearchDirt()
    {
        if (_dirt == null)
        {
            var dirt = _sensor.GetTNear<Dirt>().FirstOrDefault(x => x.IsPlaceable);
            dirt.SetPlant(this);
        }
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

    public void SetBonus(DirtType plantType)
    {
        if (plantType == DirtType.Default)
        {
            _currentBonus = new PlantRaiseMultiplier
            {
                Bonus = 0f
            };
            return;
        }

        _currentBonus = _plantRaiseData.Multipliers.FirstOrDefault(x => x.PlantType == plantType);
    }

    public void SetupDirt(Dirt dirt)
    {
        _dirt = dirt;
    }

    public void AllowMove()
    {
        //_plantMover.Enable();
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
        if (index >= _stageConfigs.Count - 1)
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
        _timeManager.OnDayChange -= Grow;

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

        var chanceValue = Random.value <= _mutliplyingChance ? 2 : 1;

        for(int i = 0; i < chanceValue; i++)
        {
            var position = transform.position + Random.insideUnitSphere;
            position.y = _dirt.transform.position.y + .1f;

            var instance = Instantiate(_prefab, position, Quaternion.identity);

            var rb = instance.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            instance.InstanceId = Guid.NewGuid().ToString();
            instance.Heritables = new Heritables
            {
                MaxImmunity = _plantHealth.MaxImmunity,
                MaxHealth = _plantHealth.MaxHealth,
            };
            instance.transform.position = position;
        }

        StartCoroutine(DecayEnumerator());
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

    private IEnumerator DecayEnumerator()
    {
        var decayShader = Shader.Find("Shader Graphs/Decay");
        var renderers = GetComponentsInChildren<Renderer>().Where(x => x.materials.Any(x => x.shader == decayShader));
        var progress = 0f;

        while(progress < 1f)
        {
            progress += Time.deltaTime;
            foreach(var renderer in renderers)
            {
                var material = renderer.materials.FirstOrDefault(x => x.shader == decayShader);
                material.SetFloat("_DecayProgress", progress);
                renderer.materials = renderer.materials;
            }

            yield return null;
        }
    }

    protected override IActivity SetupActivity(Schedule schedule)
    {
        switch (schedule.ActivityType)
        {
            case ActivityType.Sleep:
                return new PlantSleepBehaviour(_dirt.transform ?? _sleepTransform, _fetus);
            case ActivityType.Work:
                var work = _sensor.GetWorkNearby();
                return new WorkBehaviour(work);
            case ActivityType.Socialize:
                var npc = _sensor.GetNPCNearly();
                return new SocializeBehaviour(this, npc, 2f);
            default:
                return new IdleBehaviour();
        }
    }
}

[System.Serializable]
public class PlantRaiseMultiplier
{
    public DirtType PlantType;
    public float Bonus = 100f;
}

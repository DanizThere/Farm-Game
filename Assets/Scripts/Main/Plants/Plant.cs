using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlantHealth))]
[RequireComponent(typeof(PlantEmotions))]
public class Plant : PhysicObject, ISlappable
{
    public string PlantId;
    public System.Action OnExcavation = delegate { };
    public System.Action OnDeath = delegate { };
    public float Progress => _stageProgress / _currentConfig.Duration;
    public PlantStageConfig CurrentConfig => _currentConfig;
    public PlantHealth Health => _plantHealth;

    [SerializeField] private List<OnChangeStageEvent> _stageChangeEvents = new();
    [SerializeField] private List<PlantStageConfig> _stageConfigs = new();
    [SerializeField] private List<PlantRaiseMultiplier> _dirtBonus = new();
    [SerializeField] private Seed _prefab;

    [SerializeField] private AnimationCurve _seedsEvaluation;

    private PlantHealth _plantHealth;
    private PlantEmotions _plantEmotions;
    private PlantStageConfig _currentConfig;
    private PlantRaiseMultiplier _currentBonus;

    private float _stageProgress;

    private void Start()
    {
        _canGrab = false;
        var timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        _stageConfigs = _stageConfigs.OrderBy(x => x.Stage).ToList();

        _plantHealth = GetComponent<PlantHealth>();
        _plantEmotions = GetComponent<PlantEmotions>();

        if (_currentConfig == null)
        {
            _currentConfig = _stageConfigs[0];
        }

        _plantHealth.OnDeath += OnDeath;

        OnDeath += HandleDeath;
        OnExcavation += HandleExcavation;

        if (timeManager != null)
            timeManager.OnDayChange += Grow;
    }

    private void Update()
    {
        if (_plantHealth.IsAlive && !_canGrab)
            _plantHealth.Tick(Time.deltaTime, _currentConfig);
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

    public void Slap()
    {
        _plantEmotions.Emote();
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
        _stageProgress += 1 + _currentBonus!.Bonus;
        if (_stageProgress >= _currentConfig.Duration)
        {
            UpdateNextStage();
        }
    }

    private void UpdateNextStage()
    {
        var index = _stageConfigs.IndexOf(_currentConfig) + 1;
        if (index >= _stageConfigs.Count)
        {
            OnDeath?.Invoke();
        }

        var newStage = _stageConfigs[index];
        _currentConfig = newStage;
        _stageProgress = 0f;

        var onChangeStage = _stageChangeEvents.FirstOrDefault(x => x.RequiredStage == _currentConfig.Stage);
        if (onChangeStage != null)
        {
            onChangeStage.Invoke(this);
        }
    }

    private void HandleDeath()
    {
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
                instance.transform.position = position;
            }
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
}

[System.Serializable]
public class PlantRaiseMultiplier
{
    public PlantType PlantType;
    public float Bonus = 100f;
}

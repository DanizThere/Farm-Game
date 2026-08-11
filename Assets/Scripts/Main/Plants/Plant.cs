using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public System.Action OnDestroy = delegate { };

    [SerializeField] private Animator _animator;

    [SerializeField] private List<PlantRaiseMultiplier> _raiseMultiplier = new();

    [SerializeField] private PlantsLifetime _seedLife;
    [SerializeField] private PlantsLifetime _plantLife;
    [SerializeField] private PlantsLifetime _rotLife;

    private PlantsLifetime _currentLife;
    private TimeManager _timeManager;

    private void Start()
    {
        StartLife(PlantType.Chernozems);
    }


    private void Update()
    {
        //evaluate progress to grownAnimation;
    }

    public void StartLife(PlantType plantType)
    {
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();

        _seedLife.OnGrown += () => ChangeLifePeriod(_plantLife);
        _plantLife.OnGrown += () => ChangeLifePeriod(_rotLife);
        _rotLife.OnGrown += () => OnDestroy.Invoke();

        ChangeLifePeriod(_seedLife);

        var plantMultiplier = _raiseMultiplier.FirstOrDefault(x => x.PlantType == plantType);
        if (plantMultiplier != null)
        {
            _seedLife.Care(plantMultiplier.RaiseMultiplier);
        }
    }

    private void ChangeLifePeriod(PlantsLifetime lifetime)
    {
        if(_currentLife != null)
        {
            _timeManager.OnHourChange -= _currentLife.Grow;
        }
        print(lifetime);
        _currentLife = lifetime;
        //_animator.runtimeAnimatorController = _currentLife.AnimatorOverrideController;
        _currentLife.Setup();

        _timeManager.OnHourChange += _currentLife.Grow;
    }
}

[System.Serializable]
public class PlantRaiseMultiplier
{
    public PlantType PlantType;
    public float RaiseMultiplier = 1f;
}

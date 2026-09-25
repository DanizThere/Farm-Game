using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlantHealth))]
public class Plant : MonoBehaviour
{
    public System.Action OnDestroy = delegate { };

    public float Progress => _lifeProgress.Sum(x => x.Progress) / _lifeProgress.Count;

    [SerializeField] private Animator _animator;

    [SerializeField] private List<PlantRaiseMultiplier> _raiseMultiplier = new();

    [SerializeField] private List<PlantsLifetime> _lifeProgress = new();

    private PlantHealth _plantHealth;

    private PlantsLifetime _currentLife;

    private void Start()
    {
        _plantHealth = GetComponent<PlantHealth>();
        var timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        if(timeManager != null)
            timeManager.OnDayChange += Grow;
    }

    private void Grow()
    {
        _currentLife.Grow();
        print($"i grow. now i am {_currentLife} with {_currentLife.Progress} progress");
    }

    public void TryEffort(ItemObject itemObject)
    {
        foreach(var effort in itemObject.Efforts)
        {
            _plantHealth.UpdateCharacteristic(effort.Name.Name, effort.Value);
        }
    }

    public void StartLife(PlantType plantType)
    {
        for(int i = 0; i < _lifeProgress.Count; i++)
        {
            if(i == _lifeProgress.Count - 1)
            {
                _lifeProgress[i].OnGrown += () => OnDestroy.Invoke();
                break;
            }

            _lifeProgress[i].OnGrown += () => ChangeLifePeriod(_lifeProgress[i + 1]);
        }

        ChangeLifePeriod(_lifeProgress[0]);

        var plantMultiplier = _raiseMultiplier.FirstOrDefault(x => x.PlantType == plantType);
        if (plantMultiplier != null)
        {
            _currentLife.Care(plantMultiplier.RaiseMultiplier);
        }
    }

    private void ChangeLifePeriod(PlantsLifetime lifetime)
    {
        _currentLife = lifetime;
        //_animator.runtimeAnimatorController = _currentLife.AnimatorOverrideController;
        _currentLife.Setup();
    }
}

[System.Serializable]
public class PlantRaiseMultiplier
{
    public PlantType PlantType;
    public float RaiseMultiplier = 1f;
}

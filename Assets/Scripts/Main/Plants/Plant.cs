using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlantHealth))]
[RequireComponent(typeof(PlantEmotions))]
public class Plant : MonoBehaviour, ISlappable
{
    public Item InitItem;

    public System.Action OnDestroy = delegate { };

    [SerializeField] private Animator _animator;

    [SerializeField] private List<PlantsLifetime> _lifeProgress = new();

    private PlantHealth _plantHealth;
    private PlantEmotions _plantEmotions;
    private PlantsLifetime _currentLife;

    private void Start()
    {
        var timeManager = ServiceLocator.Instance.GetService<TimeManager>();

        _plantHealth = GetComponent<PlantHealth>();
        _plantEmotions = GetComponent<PlantEmotions>();

        if (timeManager != null)
            timeManager.OnDayChange += Grow;
    }

    public float GetProgress()
    {
        return _lifeProgress.Sum(x => x.Progress) / _lifeProgress.Count;
    }

    private void Grow()
    {
        _currentLife?.Grow();
    }

    public void TryEffort(ItemObject itemObject)
    {
        foreach(var effort in itemObject.Efforts)
        {
            _plantHealth.UpdateCharacteristic(effort.Name.Name, effort.GetValue());
        }
    }

    public void StartLife(PlantType plantType)
    {
        _plantHealth.Initialize();

        for (int i = 0; i < _lifeProgress.Count; i++)
        {
            if(i == _lifeProgress.Count - 1)
            {
                _lifeProgress[i].OnGrown += () => OnDestroy.Invoke();
                break;
            }

            _lifeProgress[i].OnGrown += () => ChangeLifePeriod(_lifeProgress[i + 1]);
        }

        ChangeLifePeriod(_currentLife ?? _lifeProgress[0]);
    }

    private void ChangeLifePeriod(PlantsLifetime lifetime)
    {
        _currentLife = lifetime;
        //_animator.runtimeAnimatorController = _currentLife.AnimatorOverrideController;
        _currentLife.Setup(_plantHealth);
    }

    public void Slap()
    {
        _plantEmotions.Emote();
    }
}

[System.Serializable]
public class PlantRaiseMultiplier
{
    public PlantType PlantType;
    public float RaiseMultiplier = 1f;
}

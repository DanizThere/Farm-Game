using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlantHealth))]
[RequireComponent(typeof(PlantEmotions))]
public class Plant : PhysicObject, ISlappable
{
    public Item InitItem;

    public System.Action OnDig = delegate { };
    public System.Action OnDeath = delegate { };
    public PlantsLifetime Life => _currentLife;

    [SerializeField] private Animator _animator;
    [SerializeField] private List<PlantsLifetime> _lifeProgress = new();
    [SerializeField] private List<PlantRaiseMultiplier> _dirtBonus = new();
    [SerializeField] private Seed _prefab;

    private PlantHealth _plantHealth;
    private PlantEmotions _plantEmotions;
    private PlantsLifetime _currentLife;

    private void Start()
    {
        var timeManager = ServiceLocator.Instance.GetService<TimeManager>();

        _plantHealth = GetComponent<PlantHealth>();
        _plantEmotions = GetComponent<PlantEmotions>();

        OnDeath += () =>
        {
            for (int i = 0; i < 5; i++)
            {
                var position = transform.position + Random.insideUnitSphere;
                if (Physics.CheckSphere(position, 1f))
                {
                    var instance = Instantiate(_prefab);
                    instance.transform.position = position;
                }
            }
        };

        OnDig += () =>
        {
            _canMove = true;
            _rb.useGravity = true;
            _rb.interpolation = RigidbodyInterpolation.Extrapolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            _rb.isKinematic = false;
        };

        if (timeManager != null)
            timeManager.OnDayChange += Grow;
    }

    public override void Grab(float force, CameraHandler handler, out float speedScale, HandController hand)
    {
        speedScale = 0f;
        if (!_canMove) return;
        base.Grab(force, handler, out speedScale, hand);
    }

    public override void UnGrab(HandController hand, out float speedScale)
    {
        speedScale = 1f;
        if (!_canMove) return;
        base.UnGrab(hand, out speedScale);
    }

    public float GetProgress()
    {
        return _lifeProgress.Sum(x => x.Progress) / _lifeProgress.Count;
    }

    private void Grow()
    {
        _currentLife?.Grow();
    }

    public void TryEffort(ToolItemObject itemObject)
    {
        foreach(var effort in itemObject.Efforts)
        {
            _plantHealth.UpdateCharacteristic(effort.Name, effort.GetValue());
        }
    }

    public void StartLife(PlantType plantType)
    {
        _plantHealth = GetComponent<PlantHealth>();
        _plantEmotions = GetComponent<PlantEmotions>();

        var dirtBonus = _dirtBonus.FirstOrDefault(x => x.PlantType == plantType).Bonus;

        _plantHealth.Initialize(dirtBonus);

        for (int i = 0; i < _lifeProgress.Count; i++)
        {
            int currentIndex = i;
            _lifeProgress[currentIndex].OnGrown += _plantEmotions.GrowEmote;
            if (i == _lifeProgress.Count - 1)
            {
                _lifeProgress[currentIndex].OnGrown += () => OnDeath?.Invoke();
                break;
            }
            _lifeProgress[currentIndex].OnGrown += () => ChangeLifePeriod(_lifeProgress[currentIndex + 1]);
        }

        ChangeLifePeriod(_currentLife ?? _lifeProgress[0]);
    }

    private void ChangeLifePeriod(PlantsLifetime lifetime)
    {
        _currentLife = lifetime;
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
    public float Bonus = 100f;
}

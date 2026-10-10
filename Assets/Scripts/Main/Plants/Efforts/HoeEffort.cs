using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HoeEffort : Effort
{
    public bool HoeEnabled => _hoe;

    [SerializeField] private Vector3 _startHoePosition;
    [SerializeField] private Vector3 _endHoePosition;
    [SerializeField] private Quaternion _startHoeRotation;
    [SerializeField] private Quaternion _endHoeRotation;

    [SerializeField] private HoeEffortModel _model;

    private List<HandController> _holdsHands = new();
    private const float MaxProgress = 2f;
    private float _progress = 0.0f;
    private Hoe _hoe;
    private Plant _plant;
    private bool _canEffort = true;
    private Vector3 _direction => Vector3.Lerp(_startHoePosition, _endHoePosition, _progress / MaxProgress);
    private Quaternion _rotation => Quaternion.Lerp(_startHoeRotation, _endHoeRotation, _progress / MaxProgress);

    private void Awake()
    {
        OnEnd += () =>
        {
            _canEffort = false;
            _plant.TryEffort(_hoe);

            _hoe.transform.SetParent(null);
            _model.ClearModel();

            var freeHand = _holdsHands.Where(x => !x.IsBusy).ToList();

            if (freeHand.Any())
            {
                var hand = freeHand[^1];

                hand.PickupItem(_hoe);
                _hoe.gameObject.SetActive(true);
            }
            else
            {
                var hoe = ServiceLocator.Instance
                .GetService<GameDbContext>().KeyItemObjectValue
                .Get(x => x.Type is Hoe)
                .FirstOrDefault();

                if (hoe != null) 
                {
                    var itemObject = Instantiate(hoe.Handler.GetItemObject());
                    itemObject.transform.position = transform.position + Vector3.up;
                }
            }

            _hoe = null;
        };

        _model.SetEffort(this);
    }

    public override void ResetEffort()
    {
        _canEffort = true;
        _progress = 0.0f;
    }

    public override void SetPlant(Plant plant)
    {
        _plant = plant;
    }

    public void Initialize(Hoe hoe)
    {
        _hoe = hoe;

        _model.SetModel(_hoe.CurrentModel.Value);

        _hoe.gameObject.SetActive(false);
    }

    public override void AddHand(HandController hand)
    {
        if (_holdsHands.Contains(hand)) return;

        _holdsHands.Add(hand);
    }

    public override void RemoveHand(HandController hand, System.Action onRemove)
    {
        if (!_holdsHands.Contains(hand)) return;

        _holdsHands.Remove(hand);
        onRemove?.Invoke();
    }

    public override void DoEffort()
    {
        if (!_canEffort) return;

        if (_progress > MaxProgress)
        {
            OnEnd?.Invoke();
        }

        _progress += Time.deltaTime / _model.PhysicObject.CurrentWeight;

        _model.transform.localPosition = _direction;
    }
}

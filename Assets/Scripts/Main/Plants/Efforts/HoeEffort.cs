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
    [SerializeField] private float _breakAngle = 45f;
    [SerializeField] private ParticleSystem _breakingFX;
    private Quaternion _startCameraRotation;
    private float _breakProgress;
    private Hoe _hoe;
    private Dirt _dirt;
    private bool _canEffort = true;
    private CameraHandler _handler;
    private Vector3 _direction => Vector3.Lerp(_startHoePosition, _endHoePosition, _breakProgress / 1);
    private Quaternion _rotation => Quaternion.Lerp(_startHoeRotation, _endHoeRotation, _breakProgress / 1);

    private void Awake()
    {
        OnEnd += () =>
        {
            _canEffort = false;
            _dirt.TryEffort(_hoe);

            _hoe.transform.SetParent(null);
            _model.ClearModel();

            var freeHand = _holdsHands.Where(x => !x.IsBusy).ToList();

            if (freeHand.Any())
            {
                var hand = freeHand[0];

                hand.PickupItem(_hoe);
            }
            else
            {
                var hoe = ServiceLocator.Instance
                .GetService<GameDbContext>().KeyItemObjectValue
                .Get(x => x is Hoe)
                .FirstOrDefault();

                if (hoe != null) 
                {
                    var itemObject = Instantiate(hoe);
                    itemObject.transform.position = transform.position + Vector3.up;
                    itemObject.OnThrow();
                }
            }

            _hoe = null;
        };

        _model.SetEffort(this);
    }

    private void Start()
    {
        _handler = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    public override void ResetEffort()
    {
        _canEffort = true;
        _breakProgress = 0.0f;
    }

    public override void SetDirt(Dirt dirt)
    {
        _dirt = dirt;
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
        _startCameraRotation = _handler.Camera.transform.rotation;
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

        if (_breakProgress < 1f)
        {
            var angle = Quaternion.Angle(_handler.Camera.transform.rotation, _startCameraRotation);
            if (angle > _breakAngle)
            {
                _breakProgress += Time.deltaTime * (angle / 90f);
                _breakingFX?.Emit(Mathf.RoundToInt(_breakProgress * 10));
                _model.transform.localPosition = _direction;

                if (_breakProgress > 1f)
                {
                    OnEnd?.Invoke();
                }
            }
        }
    }
}

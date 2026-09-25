using UnityEngine;

public class HoeEffort : MonoBehaviour 
{
    public bool HoeEnabled => _hoe;
    public event System.Action OnEnd = delegate { };

    [SerializeField] private Vector3 _startHoePosition;
    [SerializeField] private Vector3 _endHoePosition;
    [SerializeField] private Quaternion _startHoeRotation;

    [SerializeField] private HoeEffortModel _model;

    private const float MaxProgress = 2f;
    private float _progress = 0.0f;
    private Hoe _hoe;
    private Plant _plant;
    private bool _canEffort = true;
    private Vector3 _direction => (_endHoePosition - _startHoePosition) * _progress / MaxProgress;

    private void Awake()
    {
        OnEnd += () =>
        {
            _canEffort = false;
            _plant.TryEffort(_hoe);

            _hoe.transform.SetParent(null);
            _model.SetEffort(null);

            print("plant was efforted");

            _hoe = null;
        };

        _model.SetEffort(this);
    }

    public void ResetEffort()
    {
        _canEffort = true;
        _progress = 0.0f;
    }

    public void SetPlant(Plant plant)
    {
        _plant = plant;
    }

    public void Initialize(Hoe hoe)
    {
        _hoe = hoe;

        _model.SetModel(_hoe.CurrentModel.Value);

        _hoe.gameObject.SetActive(false);
    }

    public void Effort()
    {
        if (!_canEffort) return;

        if(_progress > MaxProgress)
        {
            OnEnd?.Invoke();
        }

        _progress += Time.deltaTime;
        print(_progress / MaxProgress);
    }
}

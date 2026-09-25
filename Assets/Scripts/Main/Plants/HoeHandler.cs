using UnityEngine;

[RequireComponent(typeof(HoeEffort))]
public class HoeHandler : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private GameObject _handleObject;
    [SerializeField] private Plant _plant;

    private HoeEffort _hoeEffort;
    private bool _canEffort = true;

    private void Awake()
    {
        _hoeEffort = GetComponent<HoeEffort>();

        _collider.enabled = false;
        _handleObject.SetActive(false);

        _hoeEffort.SetPlant(_plant);

        _hoeEffort.OnEnd += () =>
        {
            _canEffort = false;
        };
    }

    public void ShowHandle(ItemObject itemObject)
    {
        var canEffort = _canEffort && itemObject is Hoe;

        _handleObject.SetActive(canEffort);
        _collider.enabled = canEffort;
    }

    public void ResetEffort()
    {
        _canEffort = true;
        _collider.enabled = true;
    }

    public void TryEffort(ItemObject itemObject)
    {
        if (!_canEffort) return;

        if (!_hoeEffort.HoeEnabled)
        {
            if (itemObject is Hoe hoe)
            {
                _hoeEffort.Initialize(hoe);
                _collider.enabled = false;
            }
        }
    }
}

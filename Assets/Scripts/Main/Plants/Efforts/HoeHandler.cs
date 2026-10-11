using UnityEngine;

[RequireComponent(typeof(HoeEffort))]
public class HoeHandler : ItemEffortHandler
{
    private HoeEffort _hoeEffort;

    private void Awake()
    {
        _effort = GetComponent<HoeEffort>();
        _hoeEffort = _effort as HoeEffort;

        _collider.enabled = false;
        _handleObject.SetActive(false);

        _hoeEffort.SetPlant(_plant);

        _hoeEffort.OnEnd += () =>
        {
            _canEffort = false;
            HideHandle();
        };
    }

    public override void ShowHandle()
    {
        if (!_canEffort) return;
        _handleObject.SetActive(true);
        _collider.enabled = true;
    }

    public override void HideHandle()
    {
        _handleObject.SetActive(false);
        _collider.enabled = false;
    }

    public override void ResetEffort()
    {
        _canEffort = true;
        _collider.enabled = true;
        _hoeEffort.ResetEffort();
    }

    public override void TryEffort(ItemObject itemObject, System.Action onSuccess)
    {
        if (!_canEffort) return;

        if (!_hoeEffort.HoeEnabled)
        {
            if (itemObject is Hoe hoe)
            {
                _hoeEffort.Initialize(hoe);
                _collider.enabled = false;
                onSuccess?.Invoke();
            }
        }
    }

}

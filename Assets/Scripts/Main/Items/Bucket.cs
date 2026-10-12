using UnityEngine;

public class Bucket : ItemObject
{
    public float VolumePercent => _volume / _toolItem.MaxDurability;
    [SerializeField] private ParticleSystem _waterVFX;
    [SerializeField] private BucketWaterbox _bucketWaterbox;

    private float _volume = 10f;
    private bool _allowUse;

    protected override void SLStart()
    {
        base.SLStart();
        _bucketWaterbox.Initialize(this);
        _volume = _toolItem.MaxDurability;
    }

    private void Update()
    {
        _bucketWaterbox.AllowUse(_allowUse && _volume > 0f);
        if (!_allowUse || _volume < 0f) return;

        _waterVFX.Emit((int)_volume);
        _volume -= Time.deltaTime;
        _inventory?.ChangeItemWithDurability(_slotIndex, _volume);
    }

    public override void HideItem()
    {
        _inventory.ChangeItemWithDurability(_slotIndex, _volume);
    }

    public override void ShowItem()
    {
    }

    public override void StopUse()
    {
        _allowUse = false;
    }

    public override void Use()
    {
        _allowUse = true;
    }

    public void Fill(float value)
    {
        _volume += value;
        _volume = Mathf.Clamp(_volume, 0, _toolItem.MaxDurability);
    }

    public override void OnThrow()
    {
        var direction = ServiceLocator.Instance.GetService<CameraHandler>().GetDirectionForward() - transform.position;
        _rb.isKinematic = false;
        _rb.useGravity = true;

        transform.parent = null;

        _rb.AddForce(direction, ForceMode.Impulse);
    }

    public override void OnPickup(HandController handController)
    {
        _currentHand = handController;
        transform.SetParent(_currentHand.transform);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        _rb.isKinematic = true;
        _rb.useGravity = false;
    }
}

using UnityEngine;

public class Bucket : ToolItemObject
{
    [SerializeField] private ParticleSystem _waterVFX;
    [SerializeField] private BucketWaterbox _bucketWaterbox;

    private float _volume = 10f;
    private bool _allowUse;

    private ToolItem _toolItem;

    protected override void SLStart()
    {
        base.SLStart();
        _bucketWaterbox.Initialize(this);

        if(_item is ToolItem tool)
        {
            _toolItem = tool;
            _volume = _toolItem.MaxDurability;
            return;
        }

        _volume = 10f;
    }

    protected override void SLUpdate()
    {
        _bucketWaterbox.AllowUse(_allowUse && _volume > 0f);
        if (!_allowUse || _volume < 0f) return;

        _waterVFX.Emit((int)_volume);
        _volume -= Time.deltaTime;
        _inventory?.ChangeItemWithDurability(_slotIndex, _volume);
    }

    public override void HideItem()
    {
        base.HideItem();
        _inventory.ChangeItemWithDurability(_slotIndex, _volume);
    }

    public override void ShowItem()
    {
        base.ShowItem();
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
        base.OnThrow();
    }

    public override void OnPickup(HandController handController)
    {
        base.OnPickup(handController);
    }
}

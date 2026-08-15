using UnityEngine;

public class Bucket : Tool
{
    [SerializeField] private ParticleSystem _waterVFX;
    [SerializeField] private BucketUI _bucketUIPrefab;
    [SerializeField] private Transform _uiTargetPosition;

    private int _showHash = Animator.StringToHash("Show");
    private int _hideHash = Animator.StringToHash("Hide");

    private float _volume = 10f;
    private bool _allowUse;

    private BucketUI _bucketUI;

    private void Update()
    {
        if (!_allowUse || _volume < 0f) return;

        _waterVFX.Emit((int)_volume);
        _volume -= Time.deltaTime;

        _bucketUI.ShowDurability(_volume / _toolItem.MaxDurability);
    }

    public override void Setup(Inventory inventory, ToolItem toolItem, Transform parent, int slotIndex)
    {
        base.Setup(inventory, toolItem, parent, slotIndex);

        _volume = toolItem.Durability;

        if (_bucketUI) return;
        _bucketUI = Instantiate(_bucketUIPrefab, _UIParent);
        _bucketUI.Setup(_uiTargetPosition, _volume / _toolItem.MaxDurability);
    }

    public override void AltUse()
    {
        throw new System.NotImplementedException();
    }

    public override void Hide()
    {
        //_animator.SetTrigger(_hideHash);
        _inventory.ChangeItemWithDurability(_slotIndex, _volume);
        _bucketUI.Hide();
    }

    public override void Show()
    {
        //_animator.SetTrigger(_showHash);
        _bucketUI.Show();
        print("It shows");
    }

    public override void StopAltUse()
    {
        throw new System.NotImplementedException();
    }

    public override void StopUse()
    {
        _allowUse = false;
    }

    public override void Use()
    {
        _allowUse = true;
    }
}

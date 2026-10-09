using UnityEngine;

public class Bucket : ItemObject
{
    public float VolumePercent => _volume / _toolItem.MaxDurability;
    [SerializeField] private float _defaultVolume = 10f;
    [SerializeField] private ParticleSystem _waterVFX;
    [SerializeField] private BucketUI _bucketUIPrefab;
    [SerializeField] private Transform _uiTargetPosition;
    [SerializeField] private BucketWaterbox _bucketWaterbox;

    private float _volume = 10f;
    private bool _allowUse;

    private BucketUI _bucketUI;

    protected override void SLStart()
    {
        base.SLStart();
        _canSelect = false;
        _bucketWaterbox.Initialize(this);
        _volume = _defaultVolume;
    }

    private void Update()
    {
        _bucketWaterbox.AllowUse(_allowUse && _volume > 0f);
        if (!_allowUse || _volume < 0f) return;

        _waterVFX.Emit((int)_volume);
        _volume -= Time.deltaTime;

        //_bucketUI.ShowDurability(VolumePercent);
    }

    public override void Setup(Inventory inventory, ToolItem toolItem, Transform parent, int slotIndex)
    {
        base.Setup(inventory, toolItem, parent, slotIndex);

        //_volume = toolItem.Durability;

        //if (_bucketUI) return;
        //_bucketUI = Instantiate(_bucketUIPrefab, _UIParent);
        //_bucketUI.Setup(_uiTargetPosition, VolumePercent);
    }

    public override void AltUse()
    {
        throw new System.NotImplementedException();
    }

    public override void HideItem()
    {
        _inventory.ChangeItemWithDurability(_slotIndex, _volume);
        _bucketUI.Hide();
    }

    public override void ShowItem()
    {
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
        //_inventory.RemoveItem(_slotIndex, 1);
        _canSelect = true;
    }

    public override void OnPickup(HandController handController)
    {
        _currentHand = handController;
        transform.SetParent(_currentHand.transform);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        _rb.isKinematic = true;
        _rb.useGravity = false;
        _canSelect = false;

        //_inventory.AddItem(_toolItem, 1);
        //Destroy(gameObject);
    }
}

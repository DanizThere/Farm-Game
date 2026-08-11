using UnityEngine;

public class Bucket : Tool
{
    [SerializeField] private ParticleSystem _waterVFX;

    private int _showHash = Animator.StringToHash("Show");
    private int _hideHash = Animator.StringToHash("Hide");

    private float _volume = 10f;
    private bool _allowUse;

    private void Update()
    {
        if (!_allowUse || _volume < 0.1f) return;

        _waterVFX.Emit((int)_volume);
        _volume -= Time.deltaTime;
    }

    public override void AltUse()
    {
        throw new System.NotImplementedException();
    }

    public override void Hide()
    {
        //_animator.SetTrigger(_hideHash);
    }

    public override void Show()
    {
        //_animator.SetTrigger(_showHash);
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

using UnityEngine;

public class LightSource : Switcher
{
    [SerializeField] private Light _lightSpot;
    [SerializeField] private Light _lightPoint;
    [SerializeField] private Color _color;
    [Range(10f, 1000f)]
    [SerializeField] private float _defaultIntensity = 100f;
    private bool _isActive = false;

    private void Awake()
    {
        _lightSpot.color = _color;
        _lightPoint.color = _color;
    }

    private void Update()
    {
        if (_isActive)
        {
            var intensity = _defaultIntensity + Mathf.Sin(Time.time);

            _lightSpot.intensity = intensity;
            _lightPoint.intensity = intensity;
        }
            
    }

    public override void Activate()
    {
        _isActive = true;
        _lightSpot.gameObject.SetActive(_isActive);
        _lightPoint.gameObject.SetActive(_isActive);
    }

    public override void Deactivate()
    {
        _isActive = false;
        _lightSpot.gameObject.SetActive(_isActive);
        _lightPoint.gameObject.SetActive(_isActive);
    }
}

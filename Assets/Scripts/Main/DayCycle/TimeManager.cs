using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour, IService, IFreezeable
{
    public DateTime CurrentTime => _timeService.CurrentTime;

    public event Action OnSunrise
    {
        add => _timeService.OnSunrise += value;
        remove => _timeService.OnSunrise -= value;
    }

    public event Action OnSunset
    {
        add => _timeService.OnSunset += value;
        remove => _timeService.OnSunset -= value;
    }

    public event Action OnHourChange
    {
        add => _timeService.OnHourChange += value;
        remove => _timeService.OnHourChange -= value;
    }

    public event Action OnDayChange
    {
        add => _timeService.OnDayChange += value;
        remove => _timeService.OnDayChange -= value;
    }

    [SerializeField] private TimeSettings _timeSettings;

    [SerializeField] private Light _sun;
    [SerializeField] private Light _moon;

    [SerializeField] private AnimationCurve _lightIntensityCurve;
    [SerializeField] private float _maxSunIntensity = 1f;
    [SerializeField] private float _maxMoonIntensity = .5f;

    [SerializeField] private Color _dayAmbientLight;
    [SerializeField] private Color _nightAmbientLight;
    [SerializeField] private Volume _volume;

    private ColorAdjustments _colorAdjustments;

    private TimeService _timeService;

    private float _timeScale = 1f;
    private bool _set = true;

    private void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }

    public void SetTimeScale(float timeScale)
    {
        _timeScale = timeScale;
    }

    public void Setup(TimeSerialize timeSerialize = null)
    {
        var timeSettings = Instantiate(_timeSettings);
        if(timeSerialize != null)
        {
            timeSettings.TimeMultiplier = timeSerialize.TimeMultiplier;
            timeSettings.SunsetHour = timeSerialize.SunsetHour;
            timeSettings.SunriseHour = timeSerialize.SunriseHour;
            timeSettings.StartHour = timeSerialize.StartHour;
            timeSettings.StartDate = timeSerialize.CurrentDate;
        }

        _timeService = new(timeSerialize == null ? _timeSettings : timeSettings);
        _volume.profile.TryGet(out _colorAdjustments);
    }

    public void SetUpdateTime(bool set)
    {
        _set = set;
    }

    public void SkipHours(float hours)
    {
        _timeService.SkipHours(hours);
    }

    public void SpeedUp(float multiplier = 2f)
    {
        _timeService.SpeedUp(multiplier);
    }

    public void ResetSpeed()
    {
        _timeService.ResetSpeed();
    }

    public DateTime GetDate()
    {
        return _timeService.CurrentTime;
    }

    private void UpdateLightSettings()
    {
        var dotProduct = Vector3.Dot(_sun.transform.forward, Vector3.down);
        _sun.intensity = Mathf.Lerp(0, _maxSunIntensity, _lightIntensityCurve.Evaluate(dotProduct));
        _moon.intensity = Mathf.Lerp(_maxMoonIntensity, 0, _lightIntensityCurve.Evaluate(dotProduct));

        if (_colorAdjustments == null) return;

        _colorAdjustments.colorFilter.value = Color.Lerp(_nightAmbientLight, _dayAmbientLight, _lightIntensityCurve.Evaluate(dotProduct));
    }

    private void UpdateTimeOfDay()
    {
        if (!_set) return;

        _timeService.UpdateTime(Time.deltaTime * _timeScale, false);
    }

    private void RotateSun()
    {
        var rotation = _timeService.CalculateSunAngle();
        _sun.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
    }

    public void Save(ref TimeSerialize timeSerialize)
    {
        var dataTime = new TimeSerialize
        {
            SunriseHour = _timeSettings.SunriseHour,
            SunsetHour = _timeSettings.SunsetHour,
            TimeMultiplier = _timeSettings.TimeMultiplier,
            CurrentDate = _timeService.CurrentTime.Date,
            StartHour = _timeService.CurrentTime.Hour
        };

        timeSerialize = dataTime;
    }

    public void Load(TimeSerialize timeSerialize)
    {
        Setup(timeSerialize);
    }
}

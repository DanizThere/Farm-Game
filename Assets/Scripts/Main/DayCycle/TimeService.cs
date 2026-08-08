using System;
using UnityEngine;

public class TimeService
{
    private readonly TimeSettings _settings;
    private DateTime _currentTime;
    private readonly TimeSpan _sunriseTime;
    private readonly TimeSpan _sunsetTime;

    public DateTime CurrentTime => _currentTime;

    public event Action OnSunrise = delegate { };
    public event Action OnSunset = delegate { };
    public event Action OnHourChange = delegate { };

    private readonly Observable<bool> _isDayTime;
    private readonly Observable<int> _currentHour;

    public TimeService(TimeSettings settings)
    {
        _settings = settings;
        _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_settings.StartHour);
        _sunriseTime = TimeSpan.FromHours(_settings.SunriseHour);
        _sunsetTime = TimeSpan.FromHours(_settings.SunsetHour);

        _isDayTime = new Observable<bool>(IsDayTime());
        _currentHour = new Observable<int>(_currentTime.Hour);

        _isDayTime.ValueChanged += day => (day ? OnSunrise : OnSunset)?.Invoke();
        _currentHour.ValueChanged += _ => OnHourChange?.Invoke();
    }

    public void UpdateTime(float deltaTime)
    {
        _currentTime = _currentTime.AddSeconds(deltaTime * _settings.TimeMultiplier);
    }

    public float CalculateSunAngle()
    {
        var isDay = IsDayTime();
        var startDegree = isDay ? 0 : 180;
        var start = isDay ? _sunriseTime : _sunsetTime;
        var end = isDay ? + _sunsetTime : _sunriseTime;

        var totalTime = CalculateDifference(start, end);
        var elapsedTime = CalculateDifference(start, _currentTime.TimeOfDay);

        var percentage = elapsedTime.TotalMinutes / totalTime.TotalMinutes;
        return Mathf.Lerp(startDegree, startDegree + 180, (float)percentage);
    }

    private bool IsDayTime()
    {
        return _currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetTime;
    }

    private TimeSpan CalculateDifference(TimeSpan from, TimeSpan to)
    {
        var difference = to - from;

        return difference.TotalHours < 0 ? difference + TimeSpan.FromHours(24) : difference; 
    }
}

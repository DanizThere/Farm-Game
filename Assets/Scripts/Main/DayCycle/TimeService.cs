using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimeService
{
    private readonly TimeSettings _settings;
    private DateTime _currentTime;
    private readonly TimeSpan _sunriseTime;
    private readonly TimeSpan _sunStayTime;
    private readonly TimeSpan _sunsetTime;

    public DateTime CurrentTime => _currentTime;
    public DayStage CurrentDayStage => GetStage();

    public event Action OnDay = delegate { };
    public event Action OnSunrise = delegate { };
    public event Action OnSunset = delegate { };
    public event Action OnHourChange = delegate { };
    public event Action OnDayChange = delegate { };

    private readonly Observable<DayStage> _isDayTime;
    private readonly Observable<int> _currentHour;
    private readonly Observable<int> _currentDay;

    private float _timeMultiplier;

    public TimeService(TimeSettings settings)
    {
        _settings = settings;
        _timeMultiplier = settings.TimeMultiplier;
        _currentTime = _settings.StartDate.Date + TimeSpan.FromHours(_settings.StartHour);
        _sunStayTime = TimeSpan.FromHours(_settings.SunStayHour);
        _sunriseTime = TimeSpan.FromHours(_settings.SunriseHour);
        _sunsetTime = TimeSpan.FromHours(_settings.SunsetHour);

        _isDayTime = new Observable<DayStage>(GetStage());
        _currentHour = new Observable<int>(_currentTime.Hour);
        _currentDay = new Observable<int>(_currentTime.Day);

        _isDayTime.ValueChanged += day =>
        {
            switch (day)
            {
                case DayStage.Morning:
                    OnSunrise?.Invoke();
                    break;
                case DayStage.Day:
                    OnDay?.Invoke();
                    break;
                case DayStage.Evening:
                    OnSunset?.Invoke();
                    break;
            }
        };
        _currentHour.ValueChanged += _ => OnHourChange?.Invoke();
        _currentDay.ValueChanged += _ => OnDayChange?.Invoke();
    }

    public void UpdateTime(float deltaTime, bool ignoreNight = true)
    {
        if (CurrentDayStage == DayStage.Night && ignoreNight)
        {
            return;
        }
        _currentTime = _currentTime.AddSeconds(deltaTime * _timeMultiplier);
        _isDayTime.Value = GetStage();
        _currentHour.Value = _currentTime.Hour;
        _currentDay.Value = _currentTime.Day;
    }

    public void SkipHours(float hours)
    {
        var randomMinutes = Random.Range(0, 45);
        _currentTime = _currentTime.AddHours(hours);
        _currentTime = _currentTime.AddMinutes(randomMinutes);
    }

    public void SpeedUp(float multiplier = 2)
    {
        _timeMultiplier *= multiplier;
    }

    public void ResetSpeed()
    {
        _timeMultiplier = _settings.TimeMultiplier;
    }

    public float CalculateSunAngle()
    {
        var isDay = CurrentDayStage == DayStage.Day || CurrentDayStage == DayStage.Morning;
        var startDegree = isDay ? 0 : 180;
        var start = isDay ? _sunriseTime : _sunsetTime;
        var end = isDay ? + _sunsetTime : _sunriseTime;

        var totalTime = CalculateDifference(start, end);
        var elapsedTime = CalculateDifference(start, _currentTime.TimeOfDay);

        var percentage = elapsedTime.TotalMinutes / totalTime.TotalMinutes;
        return Mathf.Lerp(startDegree, startDegree + 180, (float)percentage);
    }

    private DayStage GetStage()
    {
        var timeofday = _currentTime.TimeOfDay;

        if (timeofday > _sunriseTime && timeofday < _sunStayTime)
        {
            return DayStage.Day;
        }
        if(timeofday > _sunStayTime && timeofday < _sunsetTime)
        {
            return DayStage.Evening;
        }
        if(timeofday > _sunsetTime)
        {
            return DayStage.Night;
        }

        return DayStage.Morning;
    }

    private TimeSpan CalculateDifference(TimeSpan from, TimeSpan to)
    {
        var difference = to - from;

        return difference.TotalHours < 0 ? difference + TimeSpan.FromHours(24) : difference; 
    }
}

[Flags]
public enum DayStage
{
    Morning = 1,
    Day = 2,
    Evening = 4,
    Night = 8
}
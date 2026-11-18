using UnityEngine;

public class WorkBehaviour : IActivity
{
    public ActivityType ActivityType => ActivityType.Work;
    private float _workDuration = 5f;
    private float _timer;
    private AbstractWork _work;

    public WorkBehaviour(AbstractWork work)
    {
        _work = work;
    }

    public void Enter(NPCSensor sensor)
    {
        if(_work != null)
            _work.StartWork();
    }

    public void Exit(NPCSensor sensor)
    {
        if (_work != null)
            _work.StopWork();
    }

    public bool Tick(NPCSensor sensor, float deltaTime)
    {
        sensor.IsBusy = false;
        if (_work == null) return true;

        float distance = Vector3.Distance(sensor.Agent.transform.position, _work.transform.position);
        if (distance > sensor.TalkRadius + sensor.Agent.stoppingDistance)
        {
            sensor.Agent.SetDirectionSafe(_work.transform);
            return false;
        }

        sensor.Agent.ResetPath();
        sensor.IsBusy = true;
        _timer += deltaTime;
        return _timer >= _workDuration;
    }
}
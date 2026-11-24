using UnityEngine;
using UnityEngine.AI;

public class PlantSleepBehaviour : IActivity
{
    public ActivityType ActivityType => ActivityType.Sleep;

    private Transform _sleepPosition;
    private Fetus _seed;
    private float _maxCount;

    public PlantSleepBehaviour(Transform sleepPosition, Fetus seed, float maxCount = 5)
    {
        _sleepPosition = sleepPosition;
        _seed = seed;
        _maxCount = maxCount;
    }

    public void Enter(NPCSensor sensor)
    {
        if (_sleepPosition != null) sensor.Agent.SetDirectionSafe(_sleepPosition.transform);
        sensor.IsBusy = true;
    }

    public void Exit(NPCSensor sensor)
    {
        if (_sleepPosition != null && sensor.Agent.IsNearby(_sleepPosition.transform.position))
        {
            for (int i = 0; i < _maxCount; i++)
            {
                var position = Random.insideUnitSphere + _sleepPosition.transform.position;
                position.y = _sleepPosition.transform.position.y;
                var rotation = Random.rotation;

                var instance = GameObject.Instantiate(_seed, position, rotation);
            }
        }

        sensor.Agent.ResetPath();
        sensor.IsBusy = false;
    }

    public bool Tick(NPCSensor sensor, float deltaTime)
    {
        return false;
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SocializeBehaviour : IActivity
{
    public ActivityType ActivityType => ActivityType.Socialize;
    private NPC _target;
    private NPC _owner;
    private float _elapsed;
    private float _duration;

    public SocializeBehaviour(NPC owner, NPC target, float duration)
    {
        _owner = owner;
        _target = target;
        _duration = duration;
    }

    public void Enter(NPCSensor sensor)
    {
        _elapsed = 0f;
    }

    public void Exit(NPCSensor sensor)
    {
        if (_target == null) return;
        var newRelation = Random.Range(-2, 2);

        _owner.Character.PutRelationship(_target.Character.Id, newRelation);

        _owner.Character.UpdateMood(ReactionTitle.OnTalking, newRelation);
    }

    public bool Tick(NPCSensor sensor, float deltaTime)
    {
        sensor.IsBusy = false;

        if (_target == null) return true;

        float distance = Vector3.Distance(sensor.Agent.transform.position, _target.transform.position);
        var isFar = distance > sensor.TalkRadius + sensor.Agent.stoppingDistance;
        sensor.Animator.SetBool("IsMoving", isFar);
        if (isFar)
        {
            sensor.Agent.SetDirectionSafe(_target.transform);
            return false;
        }

        sensor.Agent.ResetPath();
        _elapsed += deltaTime;
        sensor.IsBusy = true;

        return _elapsed >= _duration;
    }
}
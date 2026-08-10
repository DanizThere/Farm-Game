using UnityEngine;

public class AnimationEventStateBehaviour : StateMachineBehaviour
{
    public string EventName;
    [Range(0f, 1f)] public float TriggerTime;

    private bool _hasTriggered;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _hasTriggered = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var currentTime = stateInfo.normalizedTime % 1f;

        if(!_hasTriggered && currentTime >= TriggerTime)
        {
            NotifyReceiver(animator);
            _hasTriggered = true;
        }
    }

    private void NotifyReceiver(Animator animator)
    {
        var receiver = animator.GetComponent<AnimationEventReceiver>();
        if(receiver != null)
        {
            receiver.OnAnimationEventTriggered(EventName);
        }
    }
}

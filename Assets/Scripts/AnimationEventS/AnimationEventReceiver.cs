using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] private List<AnimationEvent> _animationEvents = new();
    public void OnAnimationEventTriggered(string eventName)
    {
        var matchingEvent = _animationEvents.FirstOrDefault(x => x.EventName == eventName);
        matchingEvent?.OnAnimationEvent?.Invoke();
    }
}
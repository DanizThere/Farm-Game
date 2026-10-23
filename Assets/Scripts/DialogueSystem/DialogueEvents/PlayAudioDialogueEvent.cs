using System;
using UnityEngine;

[Serializable]
public class PlayAudioDialogueEvent : DialogueEvent
{
    //audio service
    [SerializeField] private string _audioTag;
    public override void Invoke()
    {
        Debug.Log($"{_audioTag} was played");
    }
}

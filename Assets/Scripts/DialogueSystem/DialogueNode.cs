using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class DialogueNode
{
    public float TextSpeed = .05f;
    public bool IsSkippable = true;
    public LocalizedString NameKey;
    public LocalizedString TextKey;
    [SerializeReference] public List<DialogueEvent> OnStartEvent = new();
    [SerializeReference] public List<DialogueEvent> OnEndEvent = new();
}
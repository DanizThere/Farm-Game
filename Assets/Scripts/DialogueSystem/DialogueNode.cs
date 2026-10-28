using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    public string Id;
    public string TargetId = "all";
    public string EntityType;
    public LocalizedString OptionKey;
    public LocalizedString NameKey;
    public LocalizedString TextKey;
    public DialogueNode NextNode;
    public List<DialogueOption> Options = new();

    [SerializeReference] public List<DialogueEvent> OnStartEvent = new();
    [SerializeReference] public List<DialogueEvent> OnEndEvent = new();
    public Action OnEnd = delegate { };
}
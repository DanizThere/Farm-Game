using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    public LocalizedString NameKey;
    public LocalizedString TextKey;
    public DialogueNode NextNode;
    public List<DialogueOption> Options = new();
}

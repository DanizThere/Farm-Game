using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DialogueOption", menuName = "Dialogue/DialogueOption")]
public class DialogueOption : ScriptableObject
{
    public LocalizedString OptionKey;
    public DialogueNode NextNode;

    public Func<bool> Condition = null;
}
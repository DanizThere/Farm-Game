using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DialogueOption", menuName = "Dialogue/DialogueOption")]
public class DialogueOption : ScriptableObject
{
    public DialogueButtonType ButtonType = DialogueButtonType.Default;
    public LocalizedString OptionKey;
    public DialogueNodeGroup NextNode;

    public Func<bool> Condition = null;
}

public enum DialogueButtonType
{
    Default,
    Yellow,
    Red
}
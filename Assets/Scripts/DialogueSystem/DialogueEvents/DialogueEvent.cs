using System;
using UnityEngine;

[Serializable]
public abstract class DialogueEvent
{
    public abstract void Invoke();
}
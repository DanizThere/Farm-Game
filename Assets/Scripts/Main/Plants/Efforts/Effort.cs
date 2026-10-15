using System;
using UnityEngine;

public abstract class Effort : MonoBehaviour
{
    public Action OnEnd;

    public abstract void AddHand(HandController hand);
    public abstract void DoEffort();
    public abstract void RemoveHand(HandController hand, Action onRemove = null);
    public abstract void ResetEffort();
    public abstract void SetDirt(Dirt dirt);
}
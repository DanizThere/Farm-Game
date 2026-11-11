using System;
using UnityEngine;

[Serializable]
public class CurrentConsumableEffect
{
    public float EffectValue = 0f;
    public float Duration = 10f;
    public ConsumableEffectType EffectType;

    public float CurrentTime;
}

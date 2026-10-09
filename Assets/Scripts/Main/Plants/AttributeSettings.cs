using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributeSettings", menuName = "AttributeSettings")]
public class AttributeSettings : ScriptableObject
{
    public float MaxGrowProgress = 48f;
    public float GrowMultiplier = 1f;
}

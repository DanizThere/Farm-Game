using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributeSettings", menuName = "AttributeSettings")]
public class AttributeSettings : ScriptableObject
{
    public AttributeType AttributeType;
    public AttributeGenType AttributeGenType;

    public float TimeToGrow = 48f;
    public float GrowMultiplier = 1f;
}

public enum AttributeType
{
    Fertile,
    Juicy
}

public enum AttributeGenType
{
    Main,
    Sub
}

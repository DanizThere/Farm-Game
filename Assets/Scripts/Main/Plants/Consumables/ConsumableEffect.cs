using UnityEngine;

[CreateAssetMenu(fileName = "CE_", menuName = "ConsumableEffect")]
public class ConsumableEffect : ScriptableObject
{
    public bool IsInstantEffect = true;
    public float EffectValue = 0f;
    public float Duration = 10f;
    public ConsumableEffectType EffectType;
    public PlantAttribute RequiredAttribute;
}

public enum ConsumableEffectType
{
    Health,
    Immunity,
    Attribute
}
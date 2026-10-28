using UnityEngine;

[System.Serializable]
public class PlantHealthCharacteristic
{
    public string Name;
    public float MaxValue;
    public float MinValue;
    public float Value;

    public float GetEfficient()
    {
        if (Value < MinValue) return 0f;

        if(Value > MaxValue) return Value * 1.5f;

        return Value;
    }
}

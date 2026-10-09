using UnityEngine;

[System.Serializable]
public class PlantHealthCharacteristic
{
    public string Name;
    public float MaxValue;
    public float Value;

    public float GetEfficient()
    {
        var efficient = Value / MaxValue;

        Value = Mathf.Clamp(Value - Random.value, 0, MaxValue);
        return efficient;
    }
}

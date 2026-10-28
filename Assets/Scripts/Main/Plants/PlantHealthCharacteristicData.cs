using UnityEngine;

[CreateAssetMenu(fileName = "PlantCharacteristic", menuName = "Plant/Characterstic")]
public class PlantHealthCharacteristicData : ScriptableObject
{
    public string Name;
    public float MaxValue;
    public float DefaultValue = 2f;

    public float MinValue;
}

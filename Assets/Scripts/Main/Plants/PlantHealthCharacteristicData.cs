using UnityEngine;

[CreateAssetMenu(fileName = "PlantCharacteristic", menuName = "Plant/Characterstic")]
public class PlantHealthCharacteristicData : ScriptableObject
{
    public PlantHealthCharacteristicNameData Name;
    public float MaxValue;
    public float DefaultValue = 2f;
    public float ValueMultiplier = 1f;
}

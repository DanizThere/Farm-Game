using UnityEngine;

[CreateAssetMenu(fileName = "PlantCharacteristic", menuName = "Plant/Characterstic")]
public class PlantHealthCharacteristicData : ScriptableObject
{
    public PlantHealthCharacteristicNameData Name;
    public float MaxValue;
}

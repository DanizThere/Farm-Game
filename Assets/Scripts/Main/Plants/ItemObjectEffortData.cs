using UnityEngine;

[CreateAssetMenu(fileName = "ItemObjectEffort", menuName = "Plant/Effort")]
public class ItemObjectEffortData : ScriptableObject
{
    public PlantHealthCharacteristicNameData Name;
    [Range(-2f, 2f)]
    public float Value = 1f;
}

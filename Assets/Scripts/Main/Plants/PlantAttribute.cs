using UnityEngine;

[CreateAssetMenu(fileName = "PlantAttribute", menuName = "Plant/PlantAttribute")]
public class PlantAttribute : ScriptableObject
{
    public string Id;
    public float DecayValue = .5f;
    public float TopBorder = 100f;
}

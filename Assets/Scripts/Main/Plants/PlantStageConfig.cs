using UnityEngine;

[CreateAssetMenu(fileName = "PlantStageConfig", menuName = "Plant/PlantStageConfig")]
public class PlantStageConfig : ScriptableObject
{
    public PlantStage Stage;
    public float Duration = 3f;

    public float HealthDecayValue = 10f;
    public float ImmunityDecayValue = 10f;
}
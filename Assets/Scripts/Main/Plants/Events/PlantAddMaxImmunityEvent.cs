using UnityEngine;

[CreateAssetMenu(fileName = "PlantRemoveMaxImmunityEvent", menuName = "Plant/OnChangeStageEvent/PlantAddMaxImmunityEvent")]
public class PlantAddMaxImmunityEvent : OnChangeStageEvent
{
    public float Value;

    public override void Invoke(Plant plant)
    {
        plant.Health.MaxImmunity += Value;
    }
}

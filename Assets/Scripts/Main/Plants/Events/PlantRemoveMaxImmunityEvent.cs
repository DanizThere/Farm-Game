using UnityEngine;

[CreateAssetMenu(fileName = "PlantRemoveMaxImmunityEvent", menuName = "Plant/OnChangeStageEvent/PlantRemoveMaxImmunityEvent")]
public class PlantRemoveMaxImmunityEvent : OnChangeStageEvent
{
    public float Value;

    public override void Invoke(Plant plant)
    {
        plant.Health.MaxImmunity -= Value;
    }
}

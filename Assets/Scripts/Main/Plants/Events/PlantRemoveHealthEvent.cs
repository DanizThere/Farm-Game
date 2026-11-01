using UnityEngine;

[CreateAssetMenu(fileName = "PlantRemoveHealthEvent", menuName = "Plant/OnChangeStageEvent/PlantRemoveHealthEvent")]
public class PlantRemoveHealthEvent : OnChangeStageEvent
{
    public float Value;

    public override void Invoke(Plant plant)
    {
        plant.ApplyDamage(Value);
    }
}

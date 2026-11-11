using UnityEngine;

[CreateAssetMenu(fileName = "PlantAllowMoveEvent", menuName = "Plant/OnChangeStageEvent/PlantAllowMoveEvent")]
public class PlantAllowMoveEvent : OnChangeStageEvent
{
    public override void Invoke(Plant plant)
    {
        plant.AllowMove();
    }
}
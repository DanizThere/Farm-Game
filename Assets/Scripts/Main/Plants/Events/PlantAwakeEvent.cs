using UnityEngine;

[CreateAssetMenu(fileName = "PlantAwakeEvent", menuName = "Plant/OnChangeStageEvent/PlantAwakeEvent")]
public class PlantAwakeEvent : OnChangeStageEvent
{
    public override void Invoke(Plant plant)
    {
        plant.Animator.TriggerAwake();
    }
}

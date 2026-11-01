using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantAddHealthEvent", menuName = "Plant/OnChangeStageEvent/PlantAddHealthEvent")]
public class PlantAddHealthEvent : OnChangeStageEvent
{
    public float Value;

    public override void Invoke(Plant plant)
    {
        plant.ApplyHealth(Value);
    }
}

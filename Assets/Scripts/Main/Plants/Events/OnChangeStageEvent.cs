using UnityEngine;

public abstract class OnChangeStageEvent : ScriptableObject
{
    public PlantStage RequiredStage;

    public abstract void Invoke(Plant plant);
}
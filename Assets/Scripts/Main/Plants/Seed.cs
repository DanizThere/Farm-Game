using UnityEngine;

public class Seed : ItemObject
{
    [SerializeField] private Plant _plant;

    public Plant GetPlant() => _plant;

    public override void StopUse()
    {
    }

    public override void Use()
    {
    }
}

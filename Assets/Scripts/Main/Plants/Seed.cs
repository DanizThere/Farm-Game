using Assets.Scripts.Extenisions;
using UnityEngine;

public class Seed : ItemObject
{

    [SerializeField] private Plant _plant;

    
    public override void StopUse()
    {
    }

    public override void Use()
    {
        if (Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit, _currentHand.HandDistance))
        {
            if(hit.transform.TryGetComponent<Dirt>(out var dirt))
            {
                dirt.PlacePlant(_plant, () =>
                {
                    _currentHand.ClearItem();
                });
            }
        }

    }
}

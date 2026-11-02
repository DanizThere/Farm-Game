using Newtonsoft.Json;
using UnityEngine;

public class Seed : ItemObject
{
    [SerializeField] private Plant _plant;
    public Heritables Heritables;
    
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
                },
                Heritables);
            }
        }

    }

    protected override string GetExtraData()
    {
        return JsonConvert.SerializeObject(Heritables);
    }

    protected override void LoadData(SaveObject data)
    {
        base.LoadData(data);
        if (string.IsNullOrEmpty(data.ExtraData)) return;

        Heritables = JsonConvert.DeserializeObject<Heritables>(data.ExtraData);
    }
}

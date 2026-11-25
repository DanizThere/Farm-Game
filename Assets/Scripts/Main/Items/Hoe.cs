using Newtonsoft.Json;
using System.Linq;
using UnityEngine;

public class Hoe : ToolItemObject
{
    public override void HideItem()
    {
        base.HideItem();
    }

    public override void OnPickup(HandController handController)
    {
        base.OnPickup(handController);
    }
    public override void OnThrow()
    {
        base.OnThrow();
    }

    public override void ShowItem()
    {
        base.ShowItem();
    }

    public override void StopUse()
    {

    }

    public override void Use()
    {
        if (Raycast(out var hit))
        {
            if (hit.transform.TryGetComponent<Dirt>(out var dirt))
            {
                dirt.Dig();
                base.Use();
            }
        }
    }
}

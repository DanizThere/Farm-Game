using System;

[Serializable]
public class InventorySlot
{
    public Observable<Item> Item;
    public int Amount;
    public float Durability;
    public bool IsEmpty => Item.Value == null && Amount <= 0;

    public InventorySlot()
    {
        Item = new(null);
        Amount = 0;
        Durability = 0;

        Item.ValueChanged += value =>
        {
            if (value is ToolItem toolValue)
            {
                Durability = toolValue.Durability;
            }
        };
    }

    public InventorySlot(Item data, float dur = -1)
    {
        Item = new(data);
        if (data is ToolItem toolData)
        {
            Durability = (dur >= 0) ? dur : toolData.Durability;
        }
        else
        {
            Durability = 0;
        }
        Item.ValueChanged += value =>
        {
            if (value is ToolItem toolValue)
            {
                Durability = toolValue.Durability;
            }
        };
    }
}

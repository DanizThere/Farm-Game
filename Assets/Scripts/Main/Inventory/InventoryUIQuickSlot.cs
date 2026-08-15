using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIQuickSlot : InventorySlotUI
{
    [SerializeField] private TextMeshProUGUI _index;
    [SerializeField] private Outline _outline;

    public override void Initialize(Inventory inventory, int index)
    {
        base.Initialize(inventory, index);

        _index.text = (index + 1).ToString();
    }

    public void ShowOutline()
    {
        _outline.enabled = true;
    }

    public void HideOutline()
    {
        _outline.enabled = false;
    }
}

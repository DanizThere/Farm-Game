using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, ISelectable
{
    public int SlotIndex => _slotIndex;
    [SerializeField] private Image iconImage;

    private Inventory _inventory;
    private int _slotIndex;

    private Color _originalColor;

    public virtual void Initialize(Inventory inventory, int index)
    {
        _inventory = inventory;
        _slotIndex = index;
        _originalColor = iconImage.color;

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        var slot = _inventory.GetSlot(_slotIndex);
        if (slot.IsEmpty)
        {
            iconImage.sprite = null;
            iconImage.color = _originalColor;
        }
        else
        {
            iconImage.sprite = slot.Item.Icon;
            iconImage.color = Color.white;
        }
    }

    public InventorySlot GetSlotByIndex()
    {
        return _inventory.GetSlot(_slotIndex);
    }

    public void Show()
    {
        var slot = GetSlotByIndex();
        if (slot.IsEmpty) return;
    }

    public void Hide()
    {
    }

    public bool CanSelect()
    {
        return true;
    }
}

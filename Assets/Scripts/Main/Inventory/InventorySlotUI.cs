using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, ISelectable
{
    public int SlotIndex => _slotIndex;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    private Inventory _inventory;
    private int _slotIndex;

    private Color _originalColor;

    private SecondDimensionTipBox _secondDimensionTipBox;

    public void Initialize(Inventory inventory, int index)
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
            amountText.text = "";
        }
        else
        {
            iconImage.sprite = slot.Item.Icon;
            iconImage.color = Color.white;
            amountText.text = slot.Amount > 1 ? slot.Amount.ToString() : "";
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

        if (_secondDimensionTipBox == null)
        {
            _secondDimensionTipBox = ServiceLocator.Instance.GetService<ViewController>().Get<SecondDimensionTipBox>();
        }

        _secondDimensionTipBox.Show();
        _secondDimensionTipBox.Move(Mouse.current.position.ReadValue());
        _secondDimensionTipBox.SetInfo(slot.Item.Id, string.Empty);
    }

    public void Hide()
    {
        if (_secondDimensionTipBox == null)
        {
            _secondDimensionTipBox = ServiceLocator.Instance.GetService<ViewController>().Get<SecondDimensionTipBox>();
        }
        _secondDimensionTipBox.Hide();
    }
}

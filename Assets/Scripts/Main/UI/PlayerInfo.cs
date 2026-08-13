using System;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IView
{
    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    [SerializeField] private GameObject _panel;

    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Start()
    {
        ServiceLocator.Instance.GetService<ViewController>().Add(this);
    }

    public void Hide()
    {
        IsActive = false;
        _panel.SetActive(IsActive);
    }

    public void Show()
    {
        IsActive = true;
        _panel.SetActive(IsActive);
    }

    public void SetMoneyText(string moneyText)
    {
        _moneyText.text = moneyText;
    }
}

using System;
using TMPro;
using UnityEngine;

public class PlayerInfo : View
{
    [SerializeField] private GameObject _panel;

    [SerializeField] private TextMeshProUGUI _moneyText;

    //private void Start()
    //{
    //    ServiceLocator.Instance.GetService<ViewController>().Add(this);
    //}

    public override void Hide()
    {
        _isActive = false;
        _panel.SetActive(IsActive);
    }

    public override void Show()
    {
        _isActive = true;
        _panel.SetActive(IsActive);
    }

    public void SetMoneyText(string moneyText)
    {
        _moneyText.text = moneyText;
    }
}

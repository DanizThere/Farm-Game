using System;
using TMPro;
using UnityEngine;

public class PlayerWatchUI : View
{
    [SerializeField] private TextMeshPro _dayText;
    [SerializeField] private TextMeshPro _timeText;
    [SerializeField] private GameObject _viewObject;

    private void Awake()
    {
        _isActive.ValueChanged += _viewObject.SetActive;
    }

    public override void Hide()
    {
        _isActive.Value = false;
    }

    public override void Show()
    {
        _isActive.Value = true;
    }

    public void SetDay(string text)
    {
        _dayText.text = text;
    }

    public void SetTime(string text)
    {
        _timeText.text = text;
    }
}

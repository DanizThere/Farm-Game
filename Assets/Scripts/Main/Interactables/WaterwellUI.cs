using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaterwellUI : View
{
    [SerializeField] private CustomButton _button;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _imageFill;
    [SerializeField] private TextMeshProUGUI _text;

    private Waterwell _waterwell;

    private void Awake()
    {
        _isActive.ValueChanged += _panel.SetActive;
    }

    public override void Hide()
    {
        _isActive.Value = false;
    }

    public void Setup(Waterwell waterwell)
    {
        _waterwell = waterwell;

        _button.OnPointerDownEvent.AddListener(OnPointerDown);
    }

    public override void Show()
    {
        _isActive.Value = true;
    }

    public void UpdateText(string text)
    {
        _text.text = text;
    }

    private void OnPointerDown()
    {
    }
}
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaterwellUI : MonoBehaviour, IView
{

    public int Order { get; set; } = 20;
    public bool IsActive { get; set; } = false;
    [SerializeField] private CustomButton _button;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _imageFill;
    [SerializeField] private TextMeshProUGUI _text;

    private Waterwell _waterwell;

    public void Hide()
    {
        IsActive = false;
        _panel.SetActive(IsActive);
    }

    public void Setup(Waterwell waterwell)
    {
        _waterwell = waterwell;

        _button.OnPointerDownEvent.AddListener(OnPointerDown);
    }

    public void Show()
    {
        IsActive = true;
        _panel.SetActive(IsActive);

        var currentTool = ServiceLocator.Instance.GetService<ToolController>().CurrentTool;

        if (currentTool != null && currentTool is Bucket bucket)
            _imageFill.fillAmount = bucket.VolumePercent;
    }

    public void UpdateText(string text)
    {
        _text.text = text;
    }

    private void OnPointerDown()
    {
        var currentTool = ServiceLocator.Instance.GetService<ToolController>().CurrentTool;

        if(currentTool != null && currentTool is Bucket bucket)
        {
            _waterwell.Fill(bucket);
            _imageFill.fillAmount = bucket.VolumePercent;
        }
    }
}
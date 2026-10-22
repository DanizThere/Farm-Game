using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : View
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    public void Setup(bool showText)
    {
        _text.gameObject.SetActive(showText);
        _isActive.ValueChanged += _panel.SetActive;
    }

    public void UpdateImage(float value)
    {
        _text.text = value.ToString("0.0");

        _image.fillAmount = value;
    }
}

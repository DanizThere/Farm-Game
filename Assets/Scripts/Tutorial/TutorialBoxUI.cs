using TMPro;
using UnityEngine;

public class TutorialBoxUI : View
{
    [SerializeField] private TextMeshProUGUI _text;

    public void Setup(string text)
    {
        _text.text = text;
    }
}
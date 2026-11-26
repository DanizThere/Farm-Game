using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CheckButton : MonoBehaviour
{
    public UnityEvent OnOpen = new();
    public UnityEvent OnClose = new();
    [SerializeField] private TextMeshProUGUI _text;
    private bool _isOpen = false;

    private void Awake()
    {
        OnOpen.AddListener(() =>
        {
            _text.text = "-";
        });
        OnClose.AddListener(() =>
        {
            _text.text = "+";
        });

        var e = _isOpen ? OnOpen : OnClose;
        e?.Invoke();
    }

    public void Toggle()
    {
        _isOpen = !_isOpen;

        var e = _isOpen ? OnOpen : OnClose;
        e?.Invoke();
    }
}

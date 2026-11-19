using System;
using System.Collections.Generic;
using TMPEffects.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : View
{
    public Action OnFinish = delegate { };
    public Action OnCancel = delegate { };
    public Action OnStart = delegate { };

    public bool IsWriting => _dialogueText.IsWriting;

    [SerializeField] private TMPWriter _dialogueText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Transform _buttonPanel;
    [SerializeField] private Button _prefab;

    private List<Button> _buttons = new();
    private string _text;

    private void Start()
    {
        _dialogueText.OnFinishWriter.AddListener(_ => OnFinish?.Invoke());
        _dialogueText.OnStopWriter.AddListener(_ => OnCancel?.Invoke());
        _dialogueText.OnStartWriter.AddListener(_ => OnStart?.Invoke());
    }

    public void SetData(string text, string name, float timeDelay = .05f, bool isSkippable = true)
    {
        _text = text;
        _dialogueText.SetSkippable(isSkippable);
        _dialogueText.CurrentDelays.SetDelay(timeDelay);
        _dialogueText.SetText(_text);
        _nameText.text = name;

        foreach(var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        _buttons.Clear();
    }

    public void Stop()
    {
        if (!_dialogueText.MaySkip) return;

        _dialogueText.StopWriter();
        _dialogueText.Show(0, _text.Length);
    }

    public void InitButton(Action onClick, string optionName)
    {
        var instance = Instantiate(_prefab, _buttonPanel);
        instance.GetComponentInChildren<TextMeshProUGUI>().text = optionName;
        instance.onClick.AddListener(onClick.Invoke);
        instance.transform.SetAsLastSibling();

        _buttons.Add(instance);
    }
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : View
{
    [SerializeField] private TextMeshProUGUI _dialogueText, _nameText;
    [SerializeField] private Transform _buttonPanel;
    [SerializeField] private Button _prefab;

    private List<Button> _buttons = new();

    public void SetData(string text, string name)
    {
        _dialogueText.maxVisibleCharacters = 0;
        _dialogueText.text = text;
        _nameText.text = name;

        foreach(var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        _buttons.Clear();
    }

    public void InitButton(Action onClick, string optionName)
    {
        var instance = Instantiate(_prefab, _buttonPanel);
        instance.GetComponentInChildren<TextMeshPro>().text = optionName;
        instance.onClick.AddListener(() => onClick.Invoke());
    }

    public void SetMaxVisible(int i)
    {
        _dialogueText.maxVisibleCharacters = i;
    }

    public void ResetMaxVisible()
    {
        _dialogueText.maxVisibleCharacters = int.MaxValue;
    }
}

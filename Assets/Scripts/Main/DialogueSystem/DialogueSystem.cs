using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour, IService
{
    [SerializeField] private TextMeshProUGUI _dialogueText, _nameText;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private Button _nextNodeButton;

    private DialogueNode _currentNode;
    private DialogueNode _nextNode;

    private IEnumerator _currentCoroutine;

    private void Awake()
    {
        _nextNodeButton.onClick.AddListener(TypeNextNode);
        _nextNodeButton.gameObject.SetActive(false);
        _dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueNode startNode)
    {
        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
        _currentNode = startNode;

        if(_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        _currentCoroutine = null;
        _currentCoroutine = TypeNode(_currentNode);

        StartCoroutine(_currentCoroutine);

        _dialoguePanel.SetActive(true);
    }

    private IEnumerator TypeNode(DialogueNode startNode)
    {
        if(startNode == null)
        {
            EndDialogue();
            yield break;
        }
        if(startNode.NextNode != null)
        {
            _nextNode = startNode.NextNode;
        }

        var text = GetLocalizedString(startNode.TextKey);
        var name = GetLocalizedNameString(startNode.NameKey);

        _dialogueText.maxVisibleCharacters = 0;

        _dialogueText.text = text;
        _nameText.text = name;

        for(int i = 0; i < text.Length; i++)
        {
            _dialogueText.maxVisibleCharacters = i;
            yield return null;
        }

        if (!startNode.Options.Any())
        {
            _nextNodeButton.gameObject.SetActive(true);
            yield break;
        }
    }

    private void EndDialogue()
    {
        _dialoguePanel.SetActive(false);

        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();
    }

    private void TypeNextNode()
    {
        StopCoroutine(_currentCoroutine);

        _currentCoroutine = null;
        _currentCoroutine = TypeNode(_nextNode);

        StartCoroutine(_currentCoroutine);
    }

    private string GetLocalizedNameString(LocalizedString localizedString)
    {
        if (localizedString.IsEmpty) return string.Empty;

        var result = localizedString.GetLocalizedString();
        return string.IsNullOrEmpty(result) ? "[Not localized]" : result;
    }

    private string GetLocalizedString(LocalizedString localizedString)
    {
        if (localizedString.IsEmpty) return "[Missing]";

        var result = localizedString.GetLocalizedString();
        return string.IsNullOrEmpty(result) ? "[Not localized]" : result;
    }
}
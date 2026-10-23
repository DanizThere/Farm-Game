using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

public class DialogueSystem : MonoBehaviour, IService
{
    public Action OnEndDialogue = delegate { };

    [SerializeField] private float _typingSpeed = .05f;
    private DialogueNode _currentNode;
    private DialogueNode _nextNode;

    private IEnumerator _currentCoroutine;

    private DialogueView _view;

    private bool _isTyping = false;

    public void StartDialogue(DialogueNode startNode)
    {
        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
        _currentNode = startNode;

        if(_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        _currentCoroutine = null;
        _currentCoroutine = TypeNode(_currentNode);

        var viewController = ServiceLocator.Instance.GetService<ViewController>();
        var go = Instantiate(viewController.Get<DialogueView>(), viewController.Canvas2D.transform);
        _view = go;

        StartCoroutine(_currentCoroutine);
    }

    public void SkipTyping()
    {
        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            _view.ResetMaxVisible();

            if (_isTyping)
            {
                _currentNode?.OnEnd?.Invoke();
                _isTyping = false;
            }

            foreach (var option in _currentNode.Options)
            {
                _view.InitButton(() => TypeNode(option.NextNode), GetLocalizedString(option.OptionKey));
            }

            return;
        }

        if (!_currentNode.Options.Any())
            TypeNextNode();
    }

    private IEnumerator TypeNode(DialogueNode startNode)
    {
        if (startNode == null)
        {
            EndDialogue();
            yield break;
        }

        _currentNode = startNode;
        _nextNode = _currentNode.NextNode;

        foreach (var events in _currentNode.OnStartEvent)
        {
            events.Invoke();
        }

        var text = GetLocalizedString(_currentNode.TextKey);
        var name = GetLocalizedNameString(_currentNode.NameKey);

        _view.SetData(text, name);
        _isTyping = true;

        for (int i = 0; i < text.Length; i++)
        {
            _view.SetMaxVisible(i);
            yield return new WaitForSeconds(_typingSpeed);
        }
        _isTyping = false;

        foreach (var option in _currentNode.Options)
        {
            _view.InitButton(() => TypeNode(option.NextNode), GetLocalizedString(option.OptionKey));
        }

        foreach (var events in _currentNode.OnEndEvent)
        {
            events.Invoke();
        }

        _currentNode.OnEnd?.Invoke();
        _currentCoroutine = null;
    }

    private void EndDialogue()
    {
        OnEndDialogue?.Invoke();
        _view.Hide();
        Destroy(_view.gameObject);
        _view = null;

        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();
    }

    private void TypeNextNode()
    {
        if(_currentCoroutine != null) StopCoroutine(_currentCoroutine);

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
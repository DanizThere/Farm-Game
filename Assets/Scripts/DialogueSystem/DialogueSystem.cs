using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

public class DialogueSystem : MonoBehaviour, IService
{
    public Action OnEndDialogue = delegate { };
    private DialogueNode _currentNode;
    private DialogueNode _nextNode;

    private DialogueView _view;

    public void StartDialogue(DialogueNode startNode)
    {
        if(_view != null)
        {
            Destroy(_view.gameObject);
            _view = null;
        }

        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
        _currentNode = startNode;

        var viewController = ServiceLocator.Instance.GetService<ViewController>();
        var go = Instantiate(viewController.Get<DialogueView>(), viewController.Canvas2D.transform);
        _view = go;

        _view.OnCancel += () =>
        {
            foreach (var e in _currentNode.OnEndEvent)
            {
                e.Invoke();
            }
            _currentNode.OnEnd?.Invoke();
            InitButtons(_currentNode.Options);

        };
        _view.OnFinish += () =>
        {
            foreach (var e in _currentNode.OnEndEvent)
            {
                e.Invoke();
            }
            _currentNode.OnEnd?.Invoke();
            InitButtons(_currentNode.Options);

        };
        _view.OnStart += () =>
        {
            foreach (var e in _currentNode.OnStartEvent)
            {
                e.Invoke();
            }
        };

        TypeNode(_currentNode);
    }

    public void StartDialogue(List<DialogueNode> nodes)
    {
        if (_view != null)
        {
            Destroy(_view.gameObject);
            _view = null;
        }
        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
        var viewController = ServiceLocator.Instance.GetService<ViewController>();
        var go = Instantiate(viewController.Get<DialogueView>(), viewController.Canvas2D.transform);
        _view = go;

        _view.OnCancel += () =>
        {
            foreach (var e in _currentNode.OnEndEvent)
            {
                e.Invoke();
            }
            _currentNode.OnEnd?.Invoke();
            InitButtons(_currentNode.Options);
        };
        _view.OnFinish += () =>
        {
            foreach (var e in _currentNode.OnEndEvent)
            {
                e.Invoke();
            }
            _currentNode.OnEnd?.Invoke();
            InitButtons(_currentNode.Options);
        };
        _view.OnStart += () =>
        {
            foreach (var e in _currentNode.OnStartEvent)
            {
                e.Invoke();
            }
        };

        _view.InitButton(EndDialogue, "Exit");
    }

    public void SkipTyping()
    {
        if (_view.IsWriting)
        {
            _view.Stop();
            return;
        }

        if (!_currentNode.Options.Any())
            TypeNextNode();
    }

    private void InitButtons(List<DialogueOption> options)
    {
        foreach (var option in options)
        {
            _view.InitButton(() =>
            {
                TypeNode(option.NextNode);
            },
            option.OptionKey.GetLocalizedStringSafe());
        }
    }

    private void TypeNode(DialogueNode startNode)
    {
        if (startNode == null)
        {
            EndDialogue();
            return;
        }

        _currentNode = startNode;
        _nextNode = _currentNode.NextNode;

        foreach (var events in _currentNode.OnStartEvent)
        {
            events.Invoke();
        }

        var text = _currentNode.TextKey.GetLocalizedStringSafe();
        var name = _currentNode.NameKey.GetLocalizedStringSafe();

        _view.SetData(text, name, _currentNode.TextSpeed);
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
        TypeNode(_nextNode);
    }
}
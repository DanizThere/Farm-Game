using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

public class DialogueSystem : MonoBehaviour, IService
{
    public Action OnEndDialogue = delegate { };
    public Action OnFinish = delegate { };
    public Action OnStart = delegate { };

    private DialogueNodeGroup _currentNodeGroup;
    private DialogueNode _currentNode;

    private DialogueView _view;
    private bool _isInChoise = false;
    public void StartDialogue(DialogueNodeGroup startNode)
    {
        if(_view != null)
        {
            Destroy(_view.gameObject);
            _view = null;
        }

        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
        _currentNodeGroup = startNode;

        var viewController = ServiceLocator.Instance.GetService<ViewController>();
        var go = Instantiate(viewController.Get<DialogueView>(), viewController.Canvas2D.transform);
        _view = go;

        _view.OnFinish += () =>
        {
            foreach (var e in _currentNode.OnEndEvent)
            {
                e.Invoke();
            }
            if (_currentNode == null) return;

            var index = _currentNodeGroup.Nodes.IndexOf(_currentNode) + 1;
            if (index >= _currentNodeGroup.Nodes.Count && _currentNodeGroup.Options.Any())
            {
                InitButtons(_currentNodeGroup.Options);
                _isInChoise = true;
            }
            OnFinish?.Invoke();
        };
        _view.OnStart += () =>
        {
            foreach (var e in _currentNode.OnStartEvent)
            {
                e.Invoke();
            }
            OnStart?.Invoke();
        };

        TypeNode();
    }

    public void SkipTyping()
    {
        if (_view.IsWriting)
        {
            _view.Stop();
            return;
        }

        TypeNextNode();
    }

    private void InitButtons(List<DialogueOption> options)
    {
        foreach (var option in options)
        {
            _view.InitButton(() =>
            {
                _currentNode = null;
                _isInChoise = false;
                SetGroup(option.NextNode);
            },
            option.OptionKey.GetLocalizedStringSafe(),
            option.ButtonType);
        }
    }

    private void TypeNode()
    {
        if (_currentNodeGroup == null || !_currentNodeGroup.Nodes.Any())
        {
            EndDialogue();
            return;
        }

        if (_isInChoise) return;

        if(_currentNode == null)
        {
            _currentNode = _currentNodeGroup.Nodes[0];
        }
        else
        {
            var index = _currentNodeGroup.Nodes.IndexOf(_currentNode) + 1;
            if(index >= _currentNodeGroup.Nodes.Count)
            {
                EndDialogue();
            }
            _currentNode = _currentNodeGroup.Nodes[index];
        }

        var text = _currentNode.TextKey.GetLocalizedStringSafe();
        var name = _currentNode.NameKey.GetLocalizedStringSafe();

        _view.SetData(text, name, _currentNode.TextSpeed, _currentNode.IsSkippable);
    }

    private void SetGroup(DialogueNodeGroup group)
    {
        _currentNodeGroup = group;
        TypeNode();
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
        TypeNode();
    }
}
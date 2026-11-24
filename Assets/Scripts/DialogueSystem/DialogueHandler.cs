using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueHandler : SLMonoBehaviour
{
    public bool IsTalking => _isTalking;
    [SerializeField] private Speaker _speaker;
    [SerializeField] protected Transform _lookTarget;
    [SerializeField] protected Vector3 _cameraRotateOffset = Vector3.right;
    [SerializeField] protected List<NodeDictionary> _nodeDictionaryPrefabs = new();
    [SerializeField] protected List<DialogueOption> _options = new();

    private CameraHandler _cameraHandler;
    private DialogueSystem _dialogueSystem;
    private bool _isTalking = false;
    private List<NodeDictionary> _nodeDictionary = new();

    protected override void SLStart()
    {
        base.SLStart();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
        _dialogueSystem = ServiceLocator.Instance.GetService<DialogueSystem>();

        _dialogueSystem.OnStart += _speaker.StartTalking;
        _dialogueSystem.OnFinish += _speaker.StopTalking;

        _dialogueSystem.OnEndDialogue += () =>
        {
            _isTalking = false;
        };
        foreach(var node in _nodeDictionaryPrefabs)
        {
            var newNode = new NodeDictionary
            {
                NodeType = node.NodeType,
                Node = Instantiate(node.Node)
            };

            if(newNode.NodeType == NodeType.Start)
            {
                newNode.Node.Options.AddRange(_options);
                newNode.Node.Options.Reverse();
            }

            _nodeDictionary.Add(newNode);
        }
    }

    protected override void SLUpdate()
    {
        base.SLUpdate();
        if (_isTalking)
        {
            _cameraHandler.LookAt(_lookTarget.position + _cameraRotateOffset);
        }
    }

    public DialogueNodeGroup GetNode(NodeType type)
    {
        return _nodeDictionary.FirstOrDefault(x => x.NodeType == type).Node;
    }

    public void StartDialogue()
    {
        var start = GetNode(NodeType.Start);
        if (start == null)
        {
            print("no start node");
            return;
        }

        _dialogueSystem.StartDialogue(start);
    }
}

[Serializable]
public class NodeDictionary
{
    public NodeType NodeType;
    public DialogueNodeGroup Node;
}

public enum NodeType
{
    Start,
    Ask,
    Shop,
    Default
}
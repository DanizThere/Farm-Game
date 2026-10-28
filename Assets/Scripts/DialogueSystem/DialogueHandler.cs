using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueHandler : SLMonoBehaviour
{
    [SerializeField] private Transform _lookTarget;
    [SerializeField] private Vector3 _cameraRotateOffset = Vector3.right;

    private CameraHandler _cameraHandler;
    private DialogueSystem _dialogueSystem;
    private bool _isTalking = false;

    protected override void SLStart()
    {
        base.SLStart();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
        _dialogueSystem = ServiceLocator.Instance.GetService<DialogueSystem>();

        _dialogueSystem.OnEndDialogue += () => _isTalking = false;
    }

    protected override void SLUpdate()
    {
        base.SLUpdate();
        if (_isTalking)
        {
            _cameraHandler.LookAt(_lookTarget.position + _cameraRotateOffset);
        }
    }

    public void StartDialogue(DialogueNode node, Action onEndDialogue = null)
    {
        if(node != null)
        {
            if(onEndDialogue != null)
                _dialogueSystem.OnEndDialogue += onEndDialogue;
            _dialogueSystem.StartDialogue(node);
            _isTalking = true;
        }
    }

    public void StartDialogue(List<DialogueNode> nodes)
    {
        if (nodes.Any())
        {
            _isTalking = true;
            _dialogueSystem.StartDialogue(nodes);

        }
    }
}

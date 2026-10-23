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

    public void StartDialogue(DialogueNode node)
    {
        if(node != null)
        {
            _dialogueSystem.StartDialogue(node);
            _isTalking = true;
        }
    }
}

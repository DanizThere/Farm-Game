using UnityEngine;

public class CheckTyping : MonoBehaviour
{
    [SerializeField] private DialogueNodeGroup _testNode;
    private DialogueSystem _dialogueSystem;

    private void Start()
    {
        _dialogueSystem = ServiceLocator.Instance.GetService<DialogueSystem>();
    }

    private void Update()
    {
        if(_dialogueSystem == null)
        {
            print("no dialogue system");
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _dialogueSystem.StartDialogue(_testNode);
        }
    }
}

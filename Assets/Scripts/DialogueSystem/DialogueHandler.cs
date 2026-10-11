using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private DialogueNode _startNode;

    public void StartDialogue(DialogueSystem system)
    {
        system.StartDialogue(_startNode);
    }
}

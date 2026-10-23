using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DialogueHandler))]
[RequireComponent(typeof(CommentaryHandler))]
[RequireComponent(typeof(Character))]
public class PlantEmotions : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    private Character _character;
    private CommentaryHandler _commentaryHandler;
    private DialogueHandler _dialogueHandler;

    private void Awake()
    {
        _commentaryHandler = GetComponent<CommentaryHandler>();
        _character = GetComponent<Character>();
        _dialogueHandler = GetComponent<DialogueHandler>();
    }

    public void Emote()
    {
        var message = string.Empty;

        var node = ServiceLocator.Instance.GetService<GameDbContext>().CurrentNodes.Get().FirstOrDefault();
        if(node != null)
        {
            node.OnEnd += () =>
            {
                ServiceLocator.Instance.GetService<GameDbContext>().CurrentNodes.Remove(node);
            };
            _dialogueHandler.StartDialogue(node);
            return;
        }
        else
        {
            message = _character.UpdateMood(ReactionTitle.OnHit);
        }

        _commentaryHandler.Print(_startTransform.position, message);
    }
}

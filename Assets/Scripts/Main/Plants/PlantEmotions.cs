using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DialogueHandler))]
[RequireComponent(typeof(CommentaryHandler))]
[RequireComponent(typeof(Character))]
public class PlantEmotions : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    private Plant _plant;
    private Character _character;
    private CommentaryHandler _commentaryHandler;
    private DialogueHandler _dialogueHandler;

    private void Awake()
    {
        _plant = GetComponent<Plant>();
        _commentaryHandler = GetComponent<CommentaryHandler>();
        _character = GetComponent<Character>();
        _dialogueHandler = GetComponent<DialogueHandler>();
    }

    public void GrowEmote()
    {
        _commentaryHandler.Print(_startTransform.position, "Кажется, я немного расту");
    }

    public void Emote()
    {
        var message = string.Empty;

        var node = ServiceLocator.Instance.GetService<GameDbContext>().CurrentNodes
            .Get(x => x.EntityType == _character.EntityType &&
                (x.TargetId == _plant.InitItem.Id || x.TargetId == "all"));
        if(node != null)
        {
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

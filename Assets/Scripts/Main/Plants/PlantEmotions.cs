using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CommentaryHandler))]
[RequireComponent(typeof(Character))]
public class PlantEmotions : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    private Character _character;
    private CommentaryHandler _commentaryHandler;

    private void Awake()
    {
        _commentaryHandler = GetComponent<CommentaryHandler>();
        _character = GetComponent<Character>();
    }

    public void Emote()
    {
        var message = string.Empty;

        var node = ServiceLocator.Instance.GetService<GameDbContext>().CurrentNodes.Get().FirstOrDefault();
        if(node != null)
        {
            message = node.TextKey.GetLocalizedString();
            if (string.IsNullOrEmpty(message))
            {
                message = "not localized";
            }
            ServiceLocator.Instance.GetService<GameDbContext>().CurrentNodes.Remove(node);
        }
        else
        {
            message = _character.UpdateMood(ReactionTitle.OnHit);
        }

        _commentaryHandler.Print(_startTransform.position, message);
    }
}

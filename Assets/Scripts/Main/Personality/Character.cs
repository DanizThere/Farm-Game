using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterModel))]
public class Character : MonoBehaviour
{
    [SerializeField] private string NameId;
    [SerializeField] private Personality _personality;
    //probaply here should be standard reaction, mood must have unique reaction;
    [SerializeField] private List<Reaction> _reaction = new();

    [SerializeField] private Mood _currentMood;

    private CharacterModel _characterModel;

    private Action<Mood> _onChange = delegate { };

    private void Awake()
    {
        _onChange += ChangeMood;
        _characterModel = GetComponent<CharacterModel>();
    }


    private void ChangeMood(Mood mood)
    {
        if (mood == null) return;
        _currentMood.ResetMood();

        _currentMood = mood;
    }

    public Reaction GetReaction(ReactionTitle reactionName)
    {
        return _reaction.FirstOrDefault(x => x.ReactionName == reactionName);
    }

    public string UpdateMood(ReactionTitle reactionName)
    {
        var message = string.Empty;
        var reaction = GetReaction(reactionName);
        var value = 0f;
        if (reaction != null)
        {
            value = reaction.GetReactionValue(value, _personality);
            message = reaction.GetMessage();
        }
        else
            value *= _personality.Reactivity * _personality.ReactionRate;

        _currentMood.UpdateMood(value, _onChange);
        return message;
    }
}

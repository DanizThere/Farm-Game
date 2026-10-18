using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Personality _personality;
    //probaply here should be standard reaction, mood must have unique reaction;
    [SerializeField] private List<Reaction> _reaction = new();

    [SerializeField] private Mood _currentMood;

    private Action<Mood> _onChange = delegate { };

    private void Awake()
    {
        _onChange += ChangeMood;
    }


    private void ChangeMood(Mood mood)
    {
        _currentMood.ResetMood();

        _currentMood = mood;
    }

    public Reaction GetReaction(string reactionName)
    {
        return _reaction.FirstOrDefault(x => x.ReactionName == reactionName);
    }

    public void UpdateMood(string reactionName)
    {
        var reaction = GetReaction(reactionName);
        var value = 0f;
        if (reaction != null)
            value = reaction.GetReactionValue(value, _personality);
        else
            value *= _personality.Reactivity * _personality.ReactionRate;

        _currentMood.UpdateMood(value, _onChange);
    }
}

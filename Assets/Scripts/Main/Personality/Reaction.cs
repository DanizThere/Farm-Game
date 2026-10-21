using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "_reaction", menuName = "Character/Reaction")]
public class Reaction : ScriptableObject
{
    [SerializeField] List<LocalizedString> _comments = new();

    public ReactionTitle ReactionName;
    public float AdditionalValue;

    public float GetReactionValue(float value, Personality personality)
    {
        value += AdditionalValue;
        value *= personality.Reactivity * personality.ReactionRate;

        return value;
    }

    public string GetMessage()
    {
        var message = _comments.Random();

        if (message is null) return string.Empty;

        var localized = message.GetLocalizedString();
        if(string.IsNullOrEmpty(localized)) return "not localized\\not exists";
        return localized;
    }
}

public enum ReactionTitle
{
    OnHit,
    OnFeed,
    OnShower
}


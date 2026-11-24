using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class Reaction
{
    public List<LocalizedString> Comments = new();

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
        var message = Comments.Random();

        if (message is null) return string.Empty;

        var localized = message.GetLocalizedStringSafe();
        return localized;
    }
}

public enum ReactionTitle
{
    OnHit,
    OnFeed,
    OnShower,
    OnTalking
}


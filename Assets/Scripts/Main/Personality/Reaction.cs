using System;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    public string ReactionName;
    public float AdditionalValue;

    public float GetReactionValue(float value, Personality personality)
    {
        value += AdditionalValue;
        value *= personality.Reactivity * personality.ReactionRate;

        return value;
    }
}

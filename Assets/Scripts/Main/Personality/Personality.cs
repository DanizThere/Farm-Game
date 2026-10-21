using System;
using UnityEngine;

[Serializable]
public class Personality
{
    [Range(0f, 1f)]
    public float Sensevity = .5f;
    [Range(0f, 1f)]
    public float Reactivity = .5f;
    [Range(0f, 1f)]
    public float Activity = .5f;
    [Range(0f, 1f)]
    public float ReactionRate = .5f;
    [Range(0f, 1f)]
    public float EmotionalExcitability = .5f;
}

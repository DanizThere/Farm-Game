using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public Personality Personality;
    public List<Reaction> Reactions = new();
    public List<Mood> Moods = new();
}

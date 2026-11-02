using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFX", menuName = "SFX")]
public class SFX : ScriptableObject
{
    public string Id;
    public List<AudioClip> Clips = new();
}

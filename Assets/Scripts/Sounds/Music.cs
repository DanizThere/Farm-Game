using UnityEngine;

[CreateAssetMenu(fileName = "Music", menuName = "Music")]
public class Music : ScriptableObject
{
    public string Name;
    public AudioClip Mus;
    public MusicTime Time;
}

public enum MusicTime
{
    Day,
    Night
}

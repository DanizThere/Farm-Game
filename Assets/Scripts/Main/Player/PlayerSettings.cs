using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float Speed = .1f;
    public float LookSensevity = 10f;
}

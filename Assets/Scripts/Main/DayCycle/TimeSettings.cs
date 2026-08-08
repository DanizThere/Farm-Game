using UnityEngine;

[CreateAssetMenu(fileName = "TimeSettings", menuName = "TimeSettings")]
public class TimeSettings : ScriptableObject
{
    public float TimeMultiplier = 2000f;
    public float StartHour = 12f;
    public float SunriseHour = 6f;
    public float SunsetHour = 18f;
}

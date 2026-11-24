using System;
using UnityEngine;

[Serializable]
public class Mood
{
    public string MoodName;
    public float ValueToNegativeMood;
    public float ValueToPositiveMood;

    public string NegativeMood;
    public string PositiveMood;

    private float CurrentValue;

    public string GetMood()
    {
        var mood = CurrentValue < ValueToNegativeMood ? NegativeMood : CurrentValue > ValueToPositiveMood ? PositiveMood : string.Empty;
        return mood;
    }

    public void UpdateMood(float value, Action<string> onChange = null)
    {
        CurrentValue += value;
        if(CurrentValue > ValueToPositiveMood)
        {
            onChange?.Invoke(PositiveMood);
            return;
        }
        if(CurrentValue < ValueToPositiveMood)
        {
            onChange?.Invoke(NegativeMood);
        }
    }

    public void ResetMood()
    {
        CurrentValue = 0f;
    }
}

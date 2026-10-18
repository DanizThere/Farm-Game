using UnityEngine;

public class Mood : MonoBehaviour
{
    [SerializeField] private string _moodName;
    [SerializeField] private float _valueToNegativeMood;
    [SerializeField] private float _valueToPositiveMood;

    [SerializeField] private Mood _negativeMood;
    [SerializeField] private Mood _positiveMood;

    private float _currentValue;

    public Mood GetMood()
    {
        var mood = _currentValue < _valueToNegativeMood ? _negativeMood : _currentValue > _valueToPositiveMood ? _positiveMood : this;
        return mood;
    }

    public void UpdateMood(float value, System.Action<Mood> onChange = null)
    {
        _currentValue += value;
        if(_currentValue > _valueToPositiveMood)
        {
            onChange?.Invoke(_positiveMood);
            return;
        }
        if(_currentValue < _valueToPositiveMood)
        {
            onChange?.Invoke(_negativeMood);
        }
    }

    public void ResetMood()
    {
        _currentValue = 0f;
    }
}

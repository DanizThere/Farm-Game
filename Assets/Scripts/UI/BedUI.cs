using System;
using TMPro;
using UnityEngine;

public class BedUI : View
{
    public Action OnDestroyEvent = delegate { };
    [SerializeField] private TextMeshProUGUI _text;
    private Bed _bed;
    private float _hours;

    public void Setup(Bed bed)
    {
        _bed = bed;
    }

    public void SetHours(float hours)
    {
        _hours = hours;
        _text.text = $"{_hours} часов.";
    }

    public void Sleep()
    {
        _bed.SkipHours(_hours);
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }
}

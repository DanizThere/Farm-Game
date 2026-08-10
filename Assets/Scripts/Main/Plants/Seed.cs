using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [SerializeField] private AttributeSettings _attributeSettings;

    private float _growProgress;
    private Dictionary<string, float> _injures = new();

    private void Start()
    {
        ServiceLocator.Instance.GetService<TimeManager>().OnHourChange += Grow;
    }

    //public void Setup(AttributeSettings firstAttributeSettings, AttributeSettings secondAttributeSettings)
    //{
    //}

    public void TryFixInjure(string key)
    {
        if (!_injures.TryGetValue(key, out float value)) return;

        _injures.Remove(key);
    }

    private void Grow()
    {
        _growProgress++;
        print("its growing");

        if(_growProgress >= _attributeSettings.TimeToGrow)
        {
            ServiceLocator.Instance.GetService<TimeManager>().OnHourChange -= Grow;

            print("Is grow");
        }
    }
}

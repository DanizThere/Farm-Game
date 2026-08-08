using System.Collections.Generic;
using UnityEngine;

public class PlantsLifetime : MonoBehaviour
{
    public float Progress => _growProgress / _attributeSettings.TimeToGrow;
    [SerializeField] protected AttributeSettings _attributeSettings;

    private float _growProgress;
    private Dictionary<string, float> _injures = new();

    //public void Setup(AttributeSettings firstAttributeSettings, AttributeSettings secondAttributeSettings)
    //{
    //}

    public void TryFixInjure(string key)
    {
        if (!_injures.TryGetValue(key, out float value)) return;

        _injures.Remove(key);
    }

    public void Grow()
    {
        _growProgress++;

        if (_growProgress >= _attributeSettings.TimeToGrow)
        {
            ServiceLocator.Instance.GetService<TimeManager>().OnHourChange -= Grow;

            print("Is grow");
        }
    }
}

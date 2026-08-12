using System.Collections.Generic;
using UnityEngine;

public class PlantsLifetime : MonoBehaviour
{
    public PlantStatus PlantStatus;
    public AnimatorOverrideController AnimatorOverrideController;
    public System.Action OnGrown = delegate { };
    public float Progress => _growProgress / _attributeSettings.TimeToGrow;

    [SerializeField] protected AttributeSettings _attributeSettingsData;
    private AttributeSettings _attributeSettings;

    private float _growProgress;
    private Dictionary<string, float> _injures = new();

    public void Setup()
    {
        _attributeSettings = _attributeSettingsData;
    }

    public void AddInsure(string key, float value)
    {
        _injures.Add(key, value);
    }

    public void TryFixInjure(string key)
    {
        if (!_injures.TryGetValue(key, out float value)) return;

        _injures.Remove(key);
    }

    public void Care(float multiplier)
    {
        _attributeSettings.TimeToGrow /= multiplier;
    }

    public void Grow()
    {
        _growProgress++;
        if (Progress > .9f)
        {
            OnGrown?.Invoke();
            print("Is grown");
        }
    }
}

public enum PlantStatus
{
    Seed,
    Plant,
    Rot
}

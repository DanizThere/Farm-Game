using System.Collections.Generic;
using UnityEngine;

public class PlantsLifetime : MonoBehaviour
{
    public AnimatorOverrideController AnimatorOverrideController;
    public System.Action OnGrown = delegate { };
    public float Progress => _growProgress / _attributeSettings.MaxGrowProgress;

    [SerializeField] protected AttributeSettings _attributeSettingsData;
    private AttributeSettings _attributeSettings;
    private PlantHealth _plantHealth;

    private float _growProgress;


    public void Setup(PlantHealth health)
    {
        _attributeSettings = Instantiate(_attributeSettingsData);
        _plantHealth = health;
    }

    

    public void Care(float multiplier)
    {
        _attributeSettings.MaxGrowProgress /= multiplier;
    }

    public void Grow()
    {
        var value = _plantHealth.GetEfficient();
        print(value);

        _growProgress += value;
        if (Progress > .9f)
        {
            OnGrown?.Invoke();
            print("Is grown");
        }
    }
}

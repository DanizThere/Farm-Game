using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantHealth : MonoBehaviour
{
    public event Action OnDeath = delegate { };
    public bool IsAlive => Health > 0f;

    [HideInInspector]
    public List<CurrentPlantAttribute> PlantAttributes = new(); 

    public float Health = 100f;
    public float MaxHealth = 100f;
    [Space]
    public float Immunity = 100f;
    public float MaxImmunity = 100f;

    [SerializeField] private List<PlantAttribute> _defaultAttributes = new();
    [SerializeField] private float _naturalHealthRegen = .2f;
    [SerializeField] private float _naturalImmunityRegen = .5f;

    private void Awake()
    {
        PlantAttributes = _defaultAttributes.Select(x => new CurrentPlantAttribute
        {
            CurrentValue = 100,
            DecayValue = x.DecayValue,
            Id = x.Id,
            TopBorder = x.TopBorder,
        }).ToList();
    }

    public void Tick(float deltaTime, PlantStageConfig stage)
    {
        ModifyImmunity(_naturalImmunityRegen);

        foreach(var attribute in PlantAttributes)
        {
            attribute.CurrentValue -= attribute.DecayValue * deltaTime;
        }

        var isAttributeBelowZero = PlantAttributes.Any(x => x.CurrentValue < 0f);
        if (isAttributeBelowZero)
        {
            ModifyHealth(-(stage.HealthDecayValue * deltaTime * (MaxImmunity / Immunity)));
            return;
        }

        ModifyHealth(_naturalHealthRegen);
    }

    public void ModifyAttribute(string name, float value)
    {
        var attribute = PlantAttributes.FirstOrDefault(x => x.Id == name);
        if (attribute != null)
        {
            attribute.CurrentValue = Mathf.Max(0, attribute.CurrentValue + value);
        }
    }

    public void ModifyHealth(float value)
    {
        Health = Mathf.Clamp(Health + value, 0, MaxHealth);
        if (Health <= 0f)
        {
            print("plant is dead");
            OnDeath?.Invoke();
        }
    }

    public void ModifyImmunity(float value)
    {
        Immunity = Mathf.Clamp(Immunity + value, 0, MaxImmunity);
    }
}

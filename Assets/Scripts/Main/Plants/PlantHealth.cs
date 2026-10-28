using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantHealth : MonoBehaviour
{
    [SerializeField] List<string> _healthCharacteristicsName = new();

    private List<PlantHealthCharacteristic> _characteristics = new();
    private float _dirtBonus;

    public void Initialize(float dirtBonus = 0f)
    {
        _dirtBonus = dirtBonus;

        if (_characteristics.Any()) return;

        var characteristics = ServiceLocator.Instance.GetService<GameDbContext>()
            .PlantHealthCharacteristics.Get(x => _healthCharacteristicsName.Any(item => item == x.Name));

        foreach(var characteristic in characteristics)
        {
            AddCharacteristic(characteristic, characteristic.DefaultValue);
        }
    }

    public void AddCharacteristic(PlantHealthCharacteristicData characteristicData, float defaultValue = 0)
    {
        var characteristic = new PlantHealthCharacteristic
        {
            Name = characteristicData.Name,
            MaxValue = characteristicData.MaxValue,
            MinValue = characteristicData.MinValue,
            Value = defaultValue,
        };

        _characteristics.Add(characteristic);
    }

    public void UpdateCharacteristic(string name, float value)
    {
        var characteristic = _characteristics.FirstOrDefault(x => x.Name == name);

        if(characteristic is null)
        {
            Debug.LogWarning($"Characteristic with {name} name doesnt exist");
            return;
        }

        characteristic.Value += value; 
    }

    public float GetEfficient()
    {
        var value = (_characteristics.Sum(x => x.GetEfficient()) / _characteristics.Count) + _dirtBonus;

        return value;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantHealth : MonoBehaviour
{
    [SerializeField] List<PlantHealthCharacteristicData> _healthCharacteristics = new();

    private List<PlantHealthCharacteristic> _characteristics = new();

    public void Initialize()
    {
        if (_characteristics.Any()) return;

        foreach(var characteristic in _healthCharacteristics)
        {
            AddCharacteristic(characteristic, characteristic.DefaultValue);
        }
    }

    public void AddCharacteristic(PlantHealthCharacteristicData characteristicData, float defaultValue = 0)
    {
        var characteristic = new PlantHealthCharacteristic
        {
            Name = characteristicData.Name.Name,
            MaxValue = characteristicData.MaxValue,
            Value = defaultValue,
            ValueMultiplier = characteristicData.ValueMultiplier,
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

        characteristic.Value = Mathf.Clamp(characteristic.Value + value * characteristic.ValueMultiplier, -characteristic.MaxValue, characteristic.MaxValue);
    }

    public float GetEfficient()
    {
        var value = _characteristics.Sum(x => x.GetEfficient()) / _characteristics.Count;

        return value;
    }
}

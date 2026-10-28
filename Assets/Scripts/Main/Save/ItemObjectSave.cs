using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemObjectSave : MonoBehaviour
{
    private List<SaveObject> _objects = new();
    public void Save()
    {
        var objects = FindObjectsByType<MonoBehaviour>().OfType<ISaveableObject>().ToList();
        print(objects.Count);
        _objects = objects.Select(x => new SaveObject
        {
            InstanceId = x.InstanceId,
            Position = x.Position,
            Rotation = x.Rotation,
            TypeTag = x.TypeTag
        }).ToList();

        var data = JsonConvert.SerializeObject(_objects);
        Debug.Log(data);
    }
}

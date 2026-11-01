using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemObjectSave : MonoBehaviour
{
    private List<SaveObject> _objects = new();
    public void Save(ref List<SaveObject> list)
    {
        var objects = FindObjectsByType<MonoBehaviour>().OfType<ISaveableObject>().ToList();
        _objects = objects.Select(x => new SaveObject
        {
            InstanceId = x.InstanceId,
            Position = x.Position,
            Rotation = x.Rotation,
            TypeTag = x.TypeTag,
            ExtraData = x.ExtraData
        }).ToList();
        list = _objects;
    }

    public void Load(List<SaveObject> objects)
    {
        var go = FindObjectsByType<MonoBehaviour>().OfType<ISaveableObject>().ToList();
        foreach(var obj in objects)
        {
            var existObj = go.FirstOrDefault(x => x.InstanceId == obj.InstanceId);
            if(existObj != null)
            {
                existObj.Load(obj);
                continue;
            }

            print($"{obj.InstanceId} doesn't exist, skipped...");
        }
    }
}

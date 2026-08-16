using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonSaveService : ISaveService
{
    public void Load<T>(string key, Action<T> callback)
    {
        var path = SavePath(key);
        
        using(var streamReader = new StreamReader(path))
        {
            var json = streamReader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<T>(json);

            callback.Invoke(data);
        }
    }

    public void Save(string key, object value, Action<bool> callback = null)
    {
        var path = SavePath(key);
        var json = JsonConvert.SerializeObject(value);

        using(var streamWriter = new StreamWriter(path))
        {
            streamWriter.Write(json);
        }

        callback?.Invoke(true);
    }

    private string SavePath(string key)
    {
        return Path.Combine(Application.persistentDataPath, "saves", key);
    }
}

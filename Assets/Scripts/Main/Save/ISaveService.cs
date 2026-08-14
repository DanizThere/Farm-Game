using System;
using UnityEngine;

public interface ISaveService
{
    public void Save(string key, object value, Action<bool> callback = null);
    public void Load<T>(string key, Action<T> callback);
}
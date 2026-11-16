using System;
using UnityEngine;

[Serializable]
public class Relationship
{
    public string Main;
    public string Related;
    public int Level
    {
        get { return _level; }
        set
        {
            _level = Mathf.Clamp(value, -5, 5);
        }
    }

    private int _level;
}

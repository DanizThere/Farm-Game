using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CurrentPlantAttribute
{
    public string Id;
    public float CurrentValue = 100;
    public float TopBorder = 120;
    public float DecayValue = .5f;
}

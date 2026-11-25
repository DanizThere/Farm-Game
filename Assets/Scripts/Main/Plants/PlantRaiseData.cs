using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantRaiseData", menuName = "Plant/PlantRaiseMultiplierData")]
public class PlantRaiseData : ScriptableObject
{
    public List<PlantRaiseMultiplier> Multipliers = new();
}

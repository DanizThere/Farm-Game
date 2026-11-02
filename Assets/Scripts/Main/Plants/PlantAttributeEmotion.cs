using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantAttributeEmotion", menuName = "Plant/PlantAttributeEmotion")]
public class PlantAttributeEmotion : ScriptableObject
{
    public PlantAttribute Attribute;
    public float Periodicity = 250f;
    public float MinValueToEmote = 50f;
}

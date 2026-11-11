using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlantsRagdoll : MonoBehaviour
{
    private List<PlantRagdollPart> _parts = new();
    private Plant _plant;

    public void Initialize(Plant plant)
    {
        _parts = transform.GetComponentsInChildren<PlantRagdollPart>().ToList();
        _plant = plant;

        foreach(var part in _parts)
        {
            part.Initialize(_plant);
        }

        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        foreach(var part in _parts)
        {
            part.Enable();
        }
    }

    public void DisableRagdoll()
    {
        foreach (var part in _parts)
        {
            part.Disable();
        }
    }
}
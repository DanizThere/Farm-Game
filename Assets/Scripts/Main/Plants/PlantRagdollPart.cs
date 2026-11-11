using System;
using UnityEngine;

public class PlantRagdollPart : MonoBehaviour, ISlappable
{
    public RagdollPart RagdollPart = RagdollPart.Default;
    [SerializeField] private float _hitForce = 1f;
    private Plant _plant;
    private Rigidbody _rb;

    public void Initialize(Plant plant)
    {
        _plant = plant;
        _rb = GetComponent<Rigidbody>();
    }

    public void Enable()
    {
        _rb.isKinematic = false;
    }

    public void Disable()
    {
        _rb.isKinematic = true;
    }

    public void Slap()
    {
        var direction = ServiceLocator.Instance.GetService<CameraHandler>().GetDirection() - transform.position;
        _plant.Slap(RagdollPart, _hitForce);
        _rb.AddForceAtPosition(direction * _hitForce, transform.position, ForceMode.Impulse);
    }

    public Plant GetPlant()
    {
        return _plant;
    }
}

public enum RagdollPart
{
    Default,
    Hand,
    Leg,
    Booty,
    Head,
    Thigh
}

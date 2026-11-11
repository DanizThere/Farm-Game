using UnityEngine;

public class PlantSensor : MonoBehaviour
{
    [SerializeField] private bool _showSphere = true;
    [SerializeField] private float _reactRadius = 5f;
    private Plant _plant;

    public void Initialize(Plant plant)
    {
        _plant = plant;
    }
    
    public void SignalizeOnPhysicDeath()
    {
        var sphereCast = Physics.SphereCastAll(transform.position, _reactRadius, transform.position);

        foreach(var hit in sphereCast)
        {
            if(hit.transform.TryGetComponent<Plant>(out var plant))
            {
                if (plant.IsSleeping) continue;
                if (plant == _plant) continue;
                if (!plant.Health.IsAlive) continue;

                plant.Emotion.ReactOnPhysicDeath(_plant);
            }
        }
    }

    public void SignalizeOnViolatedDeath()
    {
        var sphereCast = Physics.SphereCastAll(transform.position, _reactRadius, transform.position);

        foreach (var hit in sphereCast)
        {
            if (hit.transform.TryGetComponent<Plant>(out var plant))
            {
                if (plant.IsSleeping) continue;
                if (plant == _plant) continue;
                if (!plant.Health.IsAlive) continue;

                plant.Emotion.ReactOnViolatedDeath(_plant);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_showSphere)
        {
            Gizmos.DrawWireSphere(transform.position, _reactRadius);
        }
    }
}

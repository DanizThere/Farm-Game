using UnityEngine;

public class PlantMedicine : ToolItemObject
{
    [SerializeField] private ConsumableEffect _effect;
    [SerializeField] private float _usings = 5f;
    private float _currentUsings;

    protected override void SLStart()
    {
        base.SLStart();
        _currentUsings = _usings;
    }

    public override void StopUse()
    {
        
    }

    public override void Use()
    {
        if(Raycast(out var hit))
        {
            if(hit.transform.TryGetComponent<PlantRagdollPart>(out var ragdollPart))
            {
                ragdollPart.GetPlant().Health.AddEffect(_effect);
                _currentUsings--;

                if(_currentUsings == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

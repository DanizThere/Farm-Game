using UnityEngine;

public class DirtPlaceholder : MonoBehaviour
{
    [SerializeField] private Dirt _dirt;
    [SerializeField] private Plant _plant;

    private void Awake()
    {
        if (!_dirt || !_plant)
        {
            Debug.LogWarning("dirt or plant dont exist");
            return;
        }

        _dirt.Plant.Value = _plant;
    }
}

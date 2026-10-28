using UnityEngine;

public class GetPlantInfo : SLMonoBehaviour
{
    private CameraHandler _cameraHandler;
    private Plant _plant;
    protected override void SLStart()
    {
        base.SLStart();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    protected override void SLUpdate()
    {
        base.SLUpdate();
        if(Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit))
        {
            hit.transform.TryGetComponent<Plant>(out _plant);
            return;
        }

        _plant = null;
    }

    private void OnGUI()
    {
        if (_plant)
        {
            GUI.TextField(new Rect(10, 10, 200, 20), $"{_plant.Life.name}: {_plant.Life.Progress} progress to 1", 25);
        }
    }
}

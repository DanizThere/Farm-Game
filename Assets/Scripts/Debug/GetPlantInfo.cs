using System.Linq;
using System.Text;
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
            var textBuilder = new StringBuilder($"{_plant.CurrentConfig.Stage}: {_plant.Progress} progress to 1\n");
            textBuilder.Append("Attributes:\n\t");
            textBuilder.Append(string.Join("\n\t", _plant.Health.PlantAttributes.Select(x => $"{x.Id}: {x.CurrentValue} value")));
            textBuilder.Append($"\nHealth: {_plant.Health.Health}/{_plant.Health.MaxHealth}");
            textBuilder.Append($"\nImmunity: {_plant.Health.Immunity}/{_plant.Health.MaxImmunity}");
            GUI.TextField(new Rect(10, 10, 1000, 500), textBuilder.ToString(), new GUIStyle { fontSize = 40 });
        }
    }
}

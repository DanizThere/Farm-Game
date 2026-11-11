using System.Linq;
using System.Text;
using UnityEngine;

public class GetPlantInfo : SLMonoBehaviour
{
    private CameraHandler _cameraHandler;
    private Plant _plant;
    private RagdollPart _ragdollPart;

    protected override void SLStart()
    {
        base.SLStart();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    protected override void SLUpdate()
    {
        base.SLUpdate();
        if(_plant != null && Input.GetKeyDown(KeyCode.F))
        {
            foreach(var attribute in _plant.Health.PlantAttributes)
            {
                attribute.CurrentValue = 100;
            }
        }

        if (_plant != null && Input.GetKeyDown(KeyCode.N))
        {
            _plant.UpdateNextStage();
        }

        if (_plant != null && Input.GetKeyDown(KeyCode.K))
        {
            _plant.Kill();
        }

        if (Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit))
        {
            if(hit.transform.TryGetComponent<PlantRagdollPart>(out var ragdoll))
            {
                _plant = ragdoll.GetPlant();
                _ragdollPart = ragdoll.RagdollPart;
                return;
            }
        }

        _plant = null;
        _ragdollPart = RagdollPart.Default;
    }

    private void OnGUI()
    {
        if (_plant)
        {
            var textBuilder = new StringBuilder($"{_plant.CurrentConfig.Stage}: {_plant.Progress} progress to 1\n");
            textBuilder.Append($"Part: {_ragdollPart}\n\t");
            textBuilder.Append("Attributes:\n\t");
            textBuilder.Append(string.Join("\n\t", _plant.Health.PlantAttributes.Select(x => $"{x.Id}: {x.CurrentValue} value")));
            textBuilder.Append($"\nHealth: {_plant.Health.Health}/{_plant.Health.MaxHealth}");
            textBuilder.Append($"\nImmunity: {_plant.Health.Immunity}/{_plant.Health.MaxImmunity}");
            GUI.TextField(new Rect(10, 10, 1000, 500), textBuilder.ToString(), new GUIStyle { fontSize = 40 });
        }
    }
}

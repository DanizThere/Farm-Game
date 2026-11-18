using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GetNPCInfo : SLMonoBehaviour
{
    private CameraHandler _cameraHandler;
    private NPC _npc;

    protected override void SLStart()
    {
        base.SLStart();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    protected override void SLUpdate()
    {
        base.SLUpdate();

        if (Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit))
        {
            if (hit.transform.TryGetComponent<NPC>(out _npc))
            {
                return;
            }
        }

        _npc = null;
    }

    private void OnGUI()
    {
        if (_npc)
        {
            var textBuilder = new StringBuilder($"{_npc.name}. CurrentActivity: {_npc.CurrentActivity} \n Can interact: {!_npc.Sensor.IsBusy}");
            GUI.TextField(new Rect(10, 510, 1000, 500), textBuilder.ToString(), new GUIStyle { fontSize = 40 });
        }
    }
}

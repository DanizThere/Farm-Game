using UnityEngine;

public class ToolController : MonoBehaviour, IService
{
    private Tool _currentTool;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = ServiceLocator.Instance.GetService<PlayerController>();

        _playerController.OnClickEvent += Use;
        _playerController.OnClickReleased += StopUse;
        _playerController.OnClickHoldedEvent += StopUse;
    }

    public void SetTool(Tool tool)
    {
        _currentTool?.Hide();
        Destroy(_currentTool?.gameObject);

        _currentTool = tool;
        _currentTool?.Show();
    }

    public void Use()
    {
        _currentTool?.Use();
    }

    public void StopUse()
    {
        _currentTool?.StopUse();
    }
}

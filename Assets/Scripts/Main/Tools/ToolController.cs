using UnityEngine;

public class ToolController : MonoBehaviour, IService
{
    public Tool CurrentTool => _currentTool;
    private Tool _currentTool;
    private PlayerController _playerController;
    private OuterWorldGameState _state;

    private void Start()
    {
        _playerController = ServiceLocator.Instance.GetService<PlayerController>();

        _state = ServiceLocator.Instance.GetService<GameStateMachine>().GetState<OuterWorldGameState>();

        if (_state == null) return;
        _state.OnStartEvent += Setup;
        _state.OnExitEvent += Dispose;
    }

    private void OnDestroy()
    {
        if (_state == null) return;
        _state.OnStartEvent -= Setup;
        _state.OnExitEvent -= Dispose;
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

    private void Setup()
    {
        _playerController.OnClickEvent += Use;
        _playerController.OnClickReleased += StopUse;
        _playerController.OnClickHoldedEvent += StopUse;
    }

    private void Dispose()
    {
        _playerController.OnClickEvent -= Use;
        _playerController.OnClickReleased -= StopUse;
        _playerController.OnClickHoldedEvent -= StopUse;
    }
}

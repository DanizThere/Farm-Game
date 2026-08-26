using UnityEngine;

public class ItemObjectController : MonoBehaviour, IService
{
    public ItemObject CurrentTool => _currentTool;
    private ItemObject _currentTool;
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

    public void SetTool(ItemObject tool, bool shouldDestroy = true)
    {
        _currentTool?.HideItem();
        if(shouldDestroy) Destroy(_currentTool?.gameObject);

        _currentTool = tool;
        _currentTool?.ShowItem();
    }

    private void Use()
    {
        _currentTool?.Use();
    }

    private void StopUse()
    {
        _currentTool?.StopUse();
    }

    private void Throw()
    {
        _currentTool?.OnThrow();
    }

    private void Setup()
    {
        _playerController.OnClickEvent += Use;
        _playerController.OnClickReleased += StopUse;
        _playerController.OnClickHoldedEvent += StopUse;
        _playerController.OnThrowEvent += Throw;
    }

    private void Dispose()
    {
        _playerController.OnClickEvent -= Use;
        _playerController.OnClickReleased -= StopUse;
        _playerController.OnClickHoldedEvent -= StopUse;
        _playerController.OnThrowEvent -= Throw;
    }
}

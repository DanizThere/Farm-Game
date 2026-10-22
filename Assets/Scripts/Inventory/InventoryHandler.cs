using UnityEngine;

public class InventoryHandler : SLMonoBehaviour
{
    private InventoryController _inventory;
    private CameraMovement _cameraMovement;
    private GameStateMachine _gameStateMachine;

    private float _showProgress = 0f;
    private ProgressSlider _slider;

    protected override void SLStart()
    {
        base.SLStart();
        _inventory = ServiceLocator.Instance.GetService<InventoryController>() ??
            throw new System.NullReferenceException($"{nameof(InventoryController)} doesn't exist");
        _cameraMovement = ServiceLocator.Instance.GetService<CameraHandler>().CameraHolder.CameraMovement ??
            throw new System.NullReferenceException($"{nameof(CameraMovement)} doesn't exist");
        _gameStateMachine = ServiceLocator.Instance.GetService<GameStateMachine>() ??
            throw new System.NullReferenceException($"{nameof(GameStateMachine)} doesn't exist");

        var go = Instantiate(ServiceLocator.Instance.GetService<ViewController>().Get<ProgressSlider>(), 
            ServiceLocator.Instance.GetService<ViewController>().Canvas2D.transform);
        _slider = go;
        _slider.Setup(showText: true);
        _slider.Hide();

        var outerState = _gameStateMachine.GetState<OuterWorldGameState>();
        outerState.OnUpdateEvent += OnUpdateOuter;
        var uiState = _gameStateMachine.GetState<InventoryGameState>();
        uiState.OnUpdateEvent += OnUpdateUI; 
    }

    private void OnUpdateOuter()
    {
        if (_cameraMovement.IsXNearBottom(10))
        {
            _slider.Show();
            _showProgress += Time.deltaTime;
            _slider.UpdateImage(_showProgress);

            if(_showProgress > 1f)
            {
                _inventory.Show();
                _showProgress = 0f;
            }
        }
        else
        {
            _showProgress = 0f;
            _slider.Hide();
        }
    }

    private void OnUpdateUI()
    {
        if (_cameraMovement.IsXNearTop(10))
        {
            _slider.Show();
            _showProgress += Time.deltaTime;
            _slider.UpdateImage(_showProgress);

            if (_showProgress > 1f)
            {
                _inventory.Hide();
                _showProgress = 0f;
            }
        }
        else
        {
            _showProgress = 0f;
            _slider.Hide();
        }
    }
}

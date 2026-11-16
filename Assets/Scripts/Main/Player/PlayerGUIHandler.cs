using UnityEngine;

public class PlayerGUIHandler : SLMonoBehaviour
{
    private PlayerGUI _gui;
    private TimeManager _timeManager;
    private InputHandler _inputHandler;

    protected override void SLStart()
    {
        base.SLStart();

        _timeManager = ServiceLocator.Instance.GetService<TimeManager>() 
            ?? throw new System.NullReferenceException();
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>()
            ?? throw new System.NullReferenceException();
        _gui = ServiceLocator.Instance.GetService<ViewController>().Get<PlayerGUI>();

        _inputHandler.OnWatchEvent += ShowTime;
        _inputHandler.OnWatchReleasedEvent += HideTime;
    }

    public void ShowTime()
    {
        var viewController = ServiceLocator.Instance.GetService<ViewController>();
        var go = Instantiate(viewController.Get<PlayerGUI>(), viewController.Canvas2D.transform);
        _gui = go;
    }

    public void HideTime()
    {
        Destroy(_gui.gameObject);
    }
}

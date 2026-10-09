using UnityEngine;

public class PlayerWatch : SLMonoBehaviour
{
    [SerializeField] private PlayerWatchUI _watch;
    [SerializeField] private HandController _hand;

    private Transform _handTransform;
    private TimeManager _timeManager;
    private CameraHolder _cameraHolder;
    private InputHandler _inputHandler;

    protected override void SLStart()
    {
        base.SLStart();

        _timeManager = ServiceLocator.Instance.GetService<TimeManager>() 
            ?? throw new System.NullReferenceException();
        _cameraHolder = ServiceLocator.Instance.GetService<CameraHandler>().CameraHolder
            ?? throw new System.NullReferenceException();
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>()
            ?? throw new System.NullReferenceException();

        _inputHandler.OnWatchEvent += ShowTime;
        _inputHandler.OnWatchReleasedEvent += HideTime;

        _handTransform = _cameraHolder?.WatchPosition;
    }

    protected override void SLUpdate()
    {
        base.SLUpdate();

        _watch.SetDay(_timeManager.CurrentTime.ToShortDateString());
        _watch.SetTime(_timeManager.CurrentTime.ToShortTimeString());
    }

    public void ShowTime()
    {
        _hand.SetTarget(_handTransform);
        _watch.Show();
    }

    public void HideTime()
    {
        _hand.ResetTarget();
        _watch.Hide();
    }
}

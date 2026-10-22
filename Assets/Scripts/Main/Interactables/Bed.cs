using System;
using System.Collections;
using UnityEngine;

public class Bed : DefaultSelectable, ISlappable
{
    [SerializeField] private Transform _cameraSleepPosition;
    private CameraHandler _cameraHandler;
    private BedUI _bedUI;
    private TimeManager _timeManager;

    protected override void SLStart()
    {
        base.SLStart();
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>() ??
            throw new NullReferenceException($"{nameof(TimeManager)} doens't exist");
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
    }


    public void SkipHours(float hours)
    {
        StartCoroutine(Sleep(hours));

        Destroy(_bedUI.gameObject);
        _bedUI = null;
    }

    public DateTime GetPossibleTime(float hours)
    {
        return _timeManager.GetDate().AddHours(hours);
    }

    public void Show()
    {
        var viewController = ServiceLocator.Instance.GetService<ViewController>();

        var prefab = viewController.Get<BedUI>();
        var go = Instantiate(prefab, viewController.Canvas2D.transform);
        _bedUI = go;
        _bedUI.Setup(this);
    }

    private IEnumerator Sleep(float hours)
    {
        _cameraHandler.CameraHolder.SetTarget(_cameraSleepPosition);
        _cameraHandler.Camera.transform.rotation = _cameraSleepPosition.rotation;

        var expectedTime = _timeManager.CurrentTime.AddHours(hours);
        _timeManager.SpeedUp();

        while(_timeManager.CurrentTime < expectedTime)
        {
            yield return null;
        }

        _timeManager.ResetSpeed();
        yield return new WaitForSeconds(2f);

        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();
        _cameraHandler.CameraHolder.ResetTarget();
    }

    public void Slap()
    {
        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
        Show();
    }
}

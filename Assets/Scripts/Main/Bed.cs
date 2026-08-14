using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bed : MonoBehaviour, ISelectable, ISelectableActions
{
    private TimeManager _timeManager;

    [SerializeField] private List<SelectableAction> _actions = new();
    [SerializeField] private BedUI _bedUI;

    [SerializeField] private float _maxDistance = 15f;

    private const string _sleepAction = "SleepAction";

    private ViewController _viewController;
    private ThirdDimensionTipBox _thirdDimensionTipBox;

    private bool _actionsIsShows = false;
    private Transform _player;

    private void Start()
    {
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        _viewController = ServiceLocator.Instance.GetService<ViewController>();
        _thirdDimensionTipBox = _viewController.Get<ThirdDimensionTipBox>();

        var sleepAction = _actions.FirstOrDefault(x => x.ActionName == _sleepAction);

        if(sleepAction != null)
        {
            sleepAction.OnClickEvent.AddListener(_bedUI.Show);
        }
    }

    private void Update()
    {
        if (_player == null) return;

        if(Vector3.Distance(transform.position, _player.transform.position) > _maxDistance)
        {
            Hide();
        }
    }

    public void SkipHours(float hours)
    {
        _timeManager.SkipHours(hours);
    }

    public DateTime GetPossibleTime(float hours)
    {
        return _timeManager.GetDate().AddHours(hours);
    }

    public void Show()
    {
        var box = _viewController.Get<ThirdDimensionTipBox>();

        box.Show();
    }

    public void Hide()
    {
        _viewController.Get<ThirdDimensionTipBox>().Hide();
        HideActions();
        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();
    }

    public void ShowActions()
    {
        if (_actionsIsShows) return;

        _actionsIsShows = true;

        if (_thirdDimensionTipBox == null)
        {
            _thirdDimensionTipBox = _viewController.Get<ThirdDimensionTipBox>();
        }

        _thirdDimensionTipBox.ShowAction(_actions);
    }

    public void HideActions()
    {
        _actionsIsShows = false;

        if (_thirdDimensionTipBox == null)
        {
            _thirdDimensionTipBox = _viewController.Get<ThirdDimensionTipBox>();
        }

        _thirdDimensionTipBox.Deselect();
    }

    public void SetTarget(Transform target)
    {
        _player = target;
    }
}

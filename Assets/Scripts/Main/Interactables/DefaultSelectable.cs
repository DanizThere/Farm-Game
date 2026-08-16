using System.Collections.Generic;
using UnityEngine;

public class DefaultSelectable : MonoBehaviour, ISelectable, ISelectableActions
{
    [SerializeField] protected List<SelectableAction> _actions = new();
    [SerializeField] protected float _maxDistance = 15f;

    protected bool _actionsIsShows = false;
    protected Transform _player;
    protected ThirdDimensionTipBox _thirdDimensionTipBox;
    protected GameStateMachine _gameStateMachine;
    protected ViewController _viewController;


    private void Start()
    {
        InitializeInStart();
    }


    private void Update()
    {
        if (_player == null) return;

        if (Vector3.Distance(transform.position, _player.transform.position) > _maxDistance)
        {
            Hide();
            _gameStateMachine.SetState<OuterWorldGameState>();
        }
    }


    public virtual void InitializeInStart()
    {
        _viewController = ServiceLocator.Instance.GetService<ViewController>();
        _thirdDimensionTipBox = _viewController.Get<ThirdDimensionTipBox>();
        _gameStateMachine = ServiceLocator.Instance.GetService<GameStateMachine>();

        foreach (var action in _actions)
        {
            if (action.ActionName == "ExitAction")
            {
                action.OnClickEvent.AddListener(() => _gameStateMachine.SetState<OuterWorldGameState>());
                continue;
            }

            action.OnClickEvent.AddListener(() => _gameStateMachine.SetState<UIGameState>());
        }
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
    }

    public void ShowActions()
    {
        if (_actionsIsShows) return;

        _actionsIsShows = true;

        if (_thirdDimensionTipBox == null)
        {
            _thirdDimensionTipBox = ServiceLocator.Instance.GetService<ViewController>().Get<ThirdDimensionTipBox>();
            return;
        }

        _thirdDimensionTipBox.ShowAction(_actions);
    }

    public void HideActions()
    {
        _actionsIsShows = false;

        if (_thirdDimensionTipBox == null)
        {
            _thirdDimensionTipBox = ServiceLocator.Instance.GetService<ViewController>().Get<ThirdDimensionTipBox>();
            return;
        }

        _thirdDimensionTipBox.Deselect();
    }

    public void SetTarget(Transform target)
    {
        _player = target;
    }
}

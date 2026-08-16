using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewController : MonoBehaviour, IService
{
    [SerializeField] private ThirdDimensionTipBox _thirdDimensionBox;
    [SerializeField] private SecondDimensionTipBox _secondDimensionBox;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private InventoryUIQuickSlots _inventoryUIQuickSlots;
    [SerializeField] private TimeView _timeView;
    [SerializeField] private BedUI _bedUI;
    [SerializeField] private WaterwellUI _waterwellUI;

    private Dictionary<Type, IView> _views = new();

    private PlayerController _playerController;

    private void Start()
    {
        _playerController = ServiceLocator.Instance.GetService<PlayerController>();

        Add(_thirdDimensionBox);
        Add(_secondDimensionBox);
        Add(_inventoryUI);
        Add(_inventoryUIQuickSlots);
        Add(_timeView);
        Add(_bedUI);
        Add(_waterwellUI);

        _playerController.OnEscapeEvent += OnEscape;
    }

    private void OnEscape()
    {
        var activeAndHighOrder = _views
            .Select(x => x.Value)
            .Where(x => x.IsActive && x.Order != 0)
            .OrderByDescending(x => x.Order)
            .ToList();

        if(activeAndHighOrder.Count < 2)
        {
            activeAndHighOrder.FirstOrDefault()?.Hide();
            ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();
        }

    }

    public void Add(IView view)
    {
        _views.Add(view.GetType(), view);
    }

    public T Get<T>() where T : IView
    {
        _views.TryGetValue(typeof(T), out var view);

        return (T)view;
    }
}

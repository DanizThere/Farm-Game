using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThirdDimensionTipBox : MonoBehaviour, IView
{
    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = false;

    [SerializeField] private RectTransform _dragParent;
    [SerializeField] private RectTransform _dragTransform;
    [SerializeField] private Transform _actionsParent;

    private List<SelectableAction> _actions = new();

    public void Hide()
    {
        IsActive = false;
        _dragTransform.gameObject.SetActive(IsActive);
        Deselect();
    }

    public void Show()
    {
        IsActive = true;
        _dragTransform.gameObject.SetActive(IsActive);
    }

    public void ShowAction(List<SelectableAction> actions)
    {
        var orderedActions = actions
            .OrderByDescending(x => x.Order)
            .ToList();

        for(int i = 0; i < orderedActions.Count; i++)
        {
            var button = Instantiate(orderedActions[i], _actionsParent);
            button.OnClickEvent = orderedActions[i].OnClickEvent;
            button.gameObject.SetActive(true);

            _actions.Add(button);
        }
    }

    public void Deselect()
    {
        foreach (var action in _actions)
        {
            Destroy(action.gameObject);
        }
        _actions.Clear();
    }
}

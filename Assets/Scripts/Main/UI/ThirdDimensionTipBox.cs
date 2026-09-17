using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThirdDimensionTipBox : View
{

    [SerializeField] private RectTransform _dragParent;
    [SerializeField] private RectTransform _dragTransform;
    [SerializeField] private Transform _actionsParent;

    private List<SelectableAction> _actions = new();

    public override void Hide()
    {
        _isActive = false;
        _dragTransform.gameObject.SetActive(IsActive);
        Deselect();
    }

    public override void Show()
    {
        _isActive = true;
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

using System;
using UnityEngine;

public interface ISelectable
{
    public void Show();
    public void Hide();
}

public interface ISelectableActions
{
    public void SetTarget(Transform target);
    public void ShowActions();
    public void HideActions();
}
using System;
using UnityEngine;

[RequireComponent(typeof(InUpdateFollower))]
public class HandController : MonoBehaviour
{
    [SerializeField] private Hand _currentHand;
    private InUpdateFollower _inUpdateFollower;

    public enum Hand
    {
        Left,
        Right
    }

    public void Initialize(Transform target)
    {
        _inUpdateFollower = GetComponent<InUpdateFollower>();
        _inUpdateFollower.SetTarget(target);
        var inputHandler = ServiceLocator.Instance.GetService<InputHandler>();

        if(_currentHand == Hand.Left)
        {
            inputHandler.OnClickEvent += OnClick;
            inputHandler.OnClickHoldedEvent += OnHold;
            inputHandler.OnClickReleased += OnHold;
        }
        else
        {
            inputHandler.OnRightClickEvent += OnClick;
            inputHandler.OnRightClickHoldedEvent += OnHold;
            inputHandler.OnRightClickReleased += OnHold;
        }
    }

    private void OnClick()
    {

    }

    private void OnHold()
    {

    }

    private void OnRelease()
    {

    }
}

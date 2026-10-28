using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BooleanInteractable : MonoBehaviour, ISlappable
{
    [SerializeField] private List<Switcher> _switchers = new();
    [SerializeField] private bool _isSlapped = false;

    private void Awake()
    {
        if (_isSlapped)
        {
            foreach (var switcher in _switchers)
            {
                switcher.Activate();
            }
        }
        else
        {
            foreach (var switcher in _switchers)
            {
                switcher.Deactivate();
            }
        }
    }

    public void Slap()
    {
        _isSlapped = !_isSlapped;

        if (_isSlapped)
        {
            foreach (var switcher in _switchers)
            {
                switcher.Activate();
            }
        }
        else
        {
            foreach (var switcher in _switchers)
            {
                switcher.Deactivate();
            }
        }
    }
}

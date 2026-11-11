using UnityEngine;

public class PlantsAnimator : MonoBehaviour
{
    [SerializeField] private string _moveNameKey = "IsMoving";
    [SerializeField] private string _awakeNameKey = "Awake";

    [SerializeField] private Animator _animator;

    public void TriggerAwake()
    {
        _animator.SetTrigger(_awakeNameKey);
    }

    public void StartMoving()
    {
        _animator.SetBool(_moveNameKey, true);
    }

    public void StopMoving()
    {
        _animator.SetBool(_moveNameKey, false);
    }

    public void EnableAnimator()
    {
        _animator.enabled = true;
    }

    public void DisableAnimator()
    {
        _animator.enabled = false;
    }
}
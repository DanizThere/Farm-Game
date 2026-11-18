using UnityEngine;

public abstract class AbstractWork : MonoBehaviour
{
    public WorkType WorkType;
    public bool IsEngaged => _isEngaged && _canInteract;
    [SerializeField] private float _delayDuration = 5f;
    private bool _isEngaged;
    private float _timer;

    private bool _canInteract => _timer >= _delayDuration;
    private void Awake()
    {
        _timer = _delayDuration;
    }

    private void Update()
    {
        if (!_canInteract) return;
        _timer += Time.deltaTime;
    }

    public virtual void StartWork()
    {
        if (IsEngaged) return;

        _isEngaged = true;
    }

    public virtual void StopWork()
    {
        _isEngaged = false;
        _timer = 0f;
    }
}

public enum WorkType
{
    Default,
}

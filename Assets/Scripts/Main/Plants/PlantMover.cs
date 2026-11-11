using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlantMover : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _radius = 10f;
    private bool _isEnable = false;
    private NavMeshAgent _agent;

    private PlantsAnimator _animator;
    private Vector3 _direction;

    private void Awake()
    {
        _animator = GetComponent<PlantsAnimator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;

        StartCoroutine(SetDirection());
    }

    private void Update()
    {
        if (!_isEnable)
        {
            return;
        }
        if (_direction == Vector3.zero) return;

        if(_agent.velocity.sqrMagnitude > .5f)
        {
            _animator.StartMoving();
        }
        else
        {
            _animator.StopMoving();
        }

        _agent.SetDestination(_direction);
    }

    public void Enable()
    {
        _isEnable = true;
        _agent.enabled = true;
    }

    public void Disable()
    {
        _isEnable = false;
        _agent.enabled = false;
    }

    private IEnumerator SetDirection()
    {
        while (true)
        {
            var randomTime = Random.value * 10;
            yield return new WaitForSeconds(randomTime);

            if (!_isEnable) continue;

            _direction = GetPoint();     
        }
    }

    private Vector3 GetPoint()
    {
        for (int i = 0; i < 30; i++)
        {
            var moveDirection = Random.insideUnitSphere * _radius + transform.position;
            if (NavMesh.SamplePosition(moveDirection, out var hit, _agent.height * .5f + .2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }
}

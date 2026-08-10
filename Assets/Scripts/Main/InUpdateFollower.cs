using UnityEngine;

public class InUpdateFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private float _followSpeed = 10;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, _followSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}

using UnityEngine;

public class DigablePlace : MonoBehaviour
{
    public DirtType DirtType;
    [SerializeField] private float _radius = 10f;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, _radius);
    }

    public bool CanDig(Vector3 point)
    {
        return (point - transform.position).sqrMagnitude <= _radius * _radius;
    }
}

using UnityEngine;

public class DrawForward : MonoBehaviour
{
    [SerializeField] private float _distance = 5f;

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red, _distance);
    }
}

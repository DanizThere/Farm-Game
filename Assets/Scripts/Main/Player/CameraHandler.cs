using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour, IService
{
    public Camera Camera => Camera.main;
    public Vector3 Position => Camera.main.transform.position;
    public Vector3 Forward => Camera.main.transform.position + Camera.main.transform.forward;

    public Vector3 GetDirection()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        return ray.direction.normalized;
    }

    public Vector3 GetDirectionForward()
    {
        Vector3 forward = Camera.main.transform.forward;
        Ray ray = Camera.main.ScreenPointToRay(forward);
        return ray.direction.normalized;
    }

    public Vector3 GetForward(float range = 1)
    {
        return Camera.main.transform.position + Camera.main.transform.forward * range;
    }
}

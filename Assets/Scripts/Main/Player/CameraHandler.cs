using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour, IService
{
    public CameraHolder CameraHolder => _holder;
    public Camera Camera { get; private set;}
    public Vector3 Position => Camera.main.transform.position;
    public Vector3 Forward => Camera.main.transform.position + Camera.main.transform.forward;

    private CameraHolder _holder;

    public void SetHolder(CameraHolder holder)
    {
        _holder = holder;
        Camera = _holder.Camera;
    }

    public Vector3 GetDirection()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.ScreenPointToRay(mousePos);
        return ray.direction.normalized;
    }

    public Vector3 GetDirectionForward()
    {
        Vector3 forward = Camera.transform.forward;
        Ray ray = Camera.ScreenPointToRay(forward);
        return ray.direction.normalized;
    }

    public Vector3 GetForward(float range = 1)
    {
        return Camera.transform.position + Camera.transform.forward * range;
    }
}

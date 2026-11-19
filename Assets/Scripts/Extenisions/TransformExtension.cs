using UnityEngine;

public static class TransformExtension
{
    public static void VectorLookAt(this Transform transform, Vector3 lookAt)
    {
        Vector3 direction = lookAt - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}

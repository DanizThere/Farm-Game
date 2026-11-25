using System.Linq;
using UnityEngine;

public class PlantRigWorker : MonoBehaviour
{
    [ContextMenu("Assign Ragdoll Parts")]
    private void AssignParts()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        var plant = GetComponent<Plant>();
        foreach(var rb in rbs)
        {
            var part = rb.gameObject.AddComponent<PlantRagdollPart>();
            part.Initialize(plant);
        }
        print($"{rbs.Count()} parts was assigned");
    }

    [ContextMenu("Assigh Projection")]
    private void AssignProjection()
    {
        var joints = GetComponentsInChildren<CharacterJoint>();
        foreach(var joint in joints)
        {
            joint.enableProjection = true;
        }
        print($"{joints.Count()} parts was assigned");

    }
}

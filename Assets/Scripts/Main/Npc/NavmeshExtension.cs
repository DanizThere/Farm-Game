using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public static class NavmeshExtension
{
    public static void SetDirectionSafe(this NavMeshAgent agent, Transform target, float maxDistance = 5f)
    {
        if (target == null) return;
        if(NavMesh.SamplePosition(target.position, out var hit, maxDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public static void SetDestinationRandomly(this NavMeshAgent agent, float radius = 5f, int tries = 30)
    {
        for (int i = 0; i < tries; i++)
        {
            var moveDirection = Random.insideUnitSphere * radius + agent.transform.position;
            if (NavMesh.SamplePosition(moveDirection, out var hit, agent.height * .5f + .2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                break;
            }
        }
    }
}

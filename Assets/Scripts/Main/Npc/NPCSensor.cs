using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCSensor : MonoBehaviour
{
    public bool IsBusy = false;

    [SerializeField] private float _maxRadius = 5f;
    private NPC _npc;
    public NavMeshAgent Agent { get; private set; }

    public void Initialize(NPC npc)
    {
        _npc = npc;
        Agent = GetComponent<NavMeshAgent>();
    }

    public NPC GetNPCNearly()
    {
        var allNpc = GetTNear<NPC>();

        if (allNpc.Count <= 1) return null;

        NPC nearNpc = null;

        foreach(var npc in allNpc)
        {
            if (npc == _npc) continue;
            if (_npc.Character.RelationshipExist(npc.Character.Id, out var level) && level < -3) continue;

            nearNpc = npc;
            break;
        }

        return nearNpc;
    }

    public List<T> GetTNear<T>() where T : Component
    {
        var allNpc = FindObjectsByType<T>().ToList();
        allNpc = allNpc.Where(x => Vector3.Distance(x.transform.position, transform.position) <= _maxRadius).ToList();

        return allNpc;
    }
}

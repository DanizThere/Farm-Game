using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(DialogueHandler))]
public class NPCSensor : MonoBehaviour
{
    public bool IsBusy = false;
    public float TalkRadius => _talkRadius;

    [SerializeField] private WorkType _workType;
    [SerializeField] private float _sensorRadius = 30f;
    [SerializeField] private float _talkRadius = 1f;
    private NPC _npc;
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    public DialogueHandler DialogueHandler { get; private set; }

    public void Initialize(NPC npc)
    {
        _npc = npc;
        DialogueHandler = GetComponent<DialogueHandler>();
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }

    public AbstractWork GetWorkNearby()
    {
        var allWorks = GetTNear<AbstractWork>();
        AbstractWork nearWork = null;
        foreach(var work in allWorks)
        {
            if (work.IsEngaged || (work.WorkType != _workType && work.WorkType != WorkType.Default)) continue;
            nearWork = work;
            break;
        }

        return nearWork;
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
        allNpc = allNpc.Where(x => Vector3.Distance(x.transform.position, transform.position) <= _sensorRadius).ToList();

        return allNpc;
    }
}

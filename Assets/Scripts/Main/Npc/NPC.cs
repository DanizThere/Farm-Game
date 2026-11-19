using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(NPCSensor))]
public class NPC : MonoBehaviour
{
    public IActivity CurrentActivity => _currentActivity;
    public Character Character => _character;
    public NPCSensor Sensor => _sensor;

    [SerializeField] private NPCSchedule _schedule;
    [SerializeField] private Transform _homeTransform;
    [SerializeField] protected Transform _sleepTransform;
    [SerializeField] private float _idleDuration = 5f;

    protected AbstractWork _work;

    protected TimeManager _timeManager;
    protected Character _character;
    protected NPCSensor _sensor;

    protected IActivity _currentActivity;
    protected DayStage _lastCheckedStage;
    protected Schedule _currentSchedule;

    protected float _idleTimer;

    private void Awake()
    {
        NPCAwake();   
    }

    private void Start()
    {
        NPCStart();
    }

    private void Update()
    {
        NPCUpdate();
    }

    public void StartDialogue()
    {
        _sensor.DialogueHandler.StartDialogue();
        //do something;
        //maybe should change to talk with player behaviour
    }

    protected virtual void NPCUpdate()
    {
        if (_sensor.DialogueHandler.IsTalking) return;

        _idleTimer += Time.deltaTime;
        if (_idleTimer < _idleDuration) return;

        if (_timeManager.CurrentStage != _lastCheckedStage)
        {
            _lastCheckedStage = _timeManager.CurrentStage;
            TrySwitchActivity(_lastCheckedStage);
        }

        if (_currentActivity == null) return;

        if (_currentActivity.Tick(_sensor, Time.deltaTime))
        {
            TrySwitchActivity(_lastCheckedStage, forced: true);
            _idleTimer = 0f;
        }
    }

    protected virtual void NPCStart()
    {
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        InitializeActivity();
    }

    protected virtual void NPCAwake()
    {
        _idleTimer = Random.Range(0, _idleDuration);
        _character = GetComponent<Character>();
        _sensor = GetComponent<NPCSensor>();

        _sensor.Initialize(this);
    }

    protected void InitializeActivity()
    {
        _lastCheckedStage = _timeManager.CurrentStage;
        TrySwitchActivity(_lastCheckedStage);
    }

    private void TrySwitchActivity(DayStage stage, bool forced = false)
    {
        var entry = _schedule.GetSchedule(stage);
        if (entry == null) return;
        if (_currentSchedule == entry && !forced) return;

        _currentSchedule = entry;

        var activity = SetupActivity(_currentSchedule);
        if (activity == null) return;

        _currentActivity?.Exit(_sensor);
        _currentActivity = activity;
        _currentActivity.Enter(_sensor);
    }

    protected virtual IActivity SetupActivity(Schedule schedule)
    {
        switch (schedule.ActivityType)
        {
            case ActivityType.Sleep:
                return new SleepBehaviour(_sleepTransform);
            case ActivityType.Work:
                var work = _sensor.GetWorkNearby();
                return new WorkBehaviour(work);
            case ActivityType.Socialize:
                var npc = _sensor.GetNPCNearly();
                return new SocializeBehaviour(this, npc, 2f);
            default:
                return new IdleBehaviour();
        }
    }
}
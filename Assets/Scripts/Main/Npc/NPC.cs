using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(NPCSensor))]
public class NPC : MonoBehaviour
{
    public Character Character => _character;
    public NPCSensor Sensor => _sensor;

    [SerializeField] private NPCSchedule _schedule;
    [SerializeField] private Transform _homeTransform;
    [SerializeField] protected Transform _sleepTransform;

    protected TimeManager _timeManager;
    protected Character _character;
    protected NPCSensor _sensor;

    protected IActivity _currentActivity;
    protected DayStage _lastCheckedStage;
    protected Schedule _currentSchedule;

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

    public bool TryInteract()
    {
        if (_currentActivity == null) return false;
        if (_currentActivity.ActivityType == ActivityType.Sleep) return false;

        //do something;

        return true;
    }

    protected virtual void NPCUpdate()
    {
        if (_timeManager.CurrentStage != _lastCheckedStage)
        {
            _lastCheckedStage = _timeManager.CurrentStage;
            TrySwitchActivity(_lastCheckedStage);
        }

        if (_currentActivity == null) return;

        if (_currentActivity.Tick(_sensor, Time.deltaTime))
        {
            TrySwitchActivity(_lastCheckedStage, forced: true);
        }
    }

    protected virtual void NPCStart()
    {
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        InitializeActivity();
    }

    protected virtual void NPCAwake()
    {
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
            case ActivityType.Socialize:
                var npc = _sensor.GetNPCNearly();
                return new SocializeBehaviour(this, npc, 2f);
            default:
                return new IdleBehaviour();
        }
    }
}
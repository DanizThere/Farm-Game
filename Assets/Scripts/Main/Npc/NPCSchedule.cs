using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/Daily Schedule", fileName = "NpcSchedule")]
public class NPCSchedule : ScriptableObject
{
    public List<Schedule> Schedules = new();

    public Schedule GetSchedule(DayStage stage)
    {
        var schedule = Schedules.FirstOrDefault(x => x.StartStage == stage);
        return schedule;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Schedules = Schedules.OrderBy(x => x.StartStage).ToList();
    }
#endif
}

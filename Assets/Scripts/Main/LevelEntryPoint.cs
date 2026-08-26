using System.Collections;
using UnityEngine;

public class LevelEntryPoint : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;


    public IEnumerator Initialize()
    {
        _timeManager.Setup();

        yield return null;
    }
}

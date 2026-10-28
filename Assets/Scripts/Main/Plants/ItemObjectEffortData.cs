using UnityEngine;

[CreateAssetMenu(fileName = "ItemObjectEffort", menuName = "Plant/Effort")]
public class ItemObjectEffortData : ScriptableObject
{
    public string Name;
    [Range(-2f, 0f)]
    public float MinValue = 0f;
    [Range(0f, 2f)]
    public float MaxValue = 1f;

    public float GetValue()
    {
        return Random.Range(MinValue, MaxValue);
    }
}

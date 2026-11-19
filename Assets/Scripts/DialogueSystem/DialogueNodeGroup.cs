using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/DialogueNode")]
public class DialogueNodeGroup : ScriptableObject
{
    public string Id;
    public EntityType EntityType = EntityType.Default;
    public List<DialogueNode> Nodes = new();
    public List<DialogueOption> Options = new();

    private void OnValidate()
    {
#if UNITY_EDITOR
        Id = name;
#endif
    }
}
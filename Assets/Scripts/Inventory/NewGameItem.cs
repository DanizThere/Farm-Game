using UnityEngine;

[CreateAssetMenu(fileName = "NewGameItem", menuName = "Items/NewGameItem")]
public class NewGameItem : ScriptableObject
{
    public Item Item;
    public int Count;
}

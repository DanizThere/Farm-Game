using UnityEngine;

public abstract class ItemObject : MonoBehaviour
{
    public Item Item => _item;
    [SerializeField] protected Item _item;

    public abstract void Show();
    public abstract void Hide();
    public abstract void Use();
}

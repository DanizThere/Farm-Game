using System;
using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    public Item Item { get; private set; }
    public string Hand {  get; private set; }
    public void SetItem(Item item, string hand)
    {
        Item = item;
        Hand = hand;
    }
}
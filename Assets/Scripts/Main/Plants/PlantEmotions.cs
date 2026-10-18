using System;
using UnityEngine;

[RequireComponent(typeof(CommentaryHandler))]
public class PlantEmotions : MonoBehaviour
{
    private CommentaryHandler _commentaryHandler;

    private void Awake()
    {
        _commentaryHandler = GetComponent<CommentaryHandler>();
    }

    public void Emote()
    {
        _commentaryHandler.Print(transform.up);
        print("i feel a lil buzy");
    }
}

using UnityEngine;

public class DoorObject : DefaultSelectable, ISlappable
{
    [SerializeField] private Door _door;
    public void Slap()
    {
        var player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            var dot = Vector3.Dot(player.BodyTransform.forward, (transform.forward - player.BodyTransform.position).normalized);
            _door.Open(dot);
        }
    }
}

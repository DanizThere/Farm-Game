using UnityEngine;
using UnityEngine.UIElements;

public class DoorObject : DefaultSelectable, ISlappable
{
    [SerializeField] private Door _door;
    public void Slap()
    {
        var player = FindAnyObjectByType<PlayerController>();
        if (player == null) return;

        var doorPos = transform.position;
        var playerPos = player.BodyTransform.position;

        var toPlayer = playerPos - doorPos;
        toPlayer.y = 0f;

        if (toPlayer.sqrMagnitude < 0.0001f)
        {
            _door.Open(1f);
            return;
        }
        toPlayer.Normalize();

        var doorNormal = transform.forward;
        doorNormal.y = 0f;
        doorNormal.Normalize();

        var side = Vector3.Cross(doorNormal, toPlayer).y;
        var direction = Mathf.Abs(side) < 0.001f ? 1f : Mathf.Sign(side);

        _door.Open(direction);
    }
}

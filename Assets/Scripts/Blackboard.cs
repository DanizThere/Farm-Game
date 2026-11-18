using UnityEngine;

public class Blackboard : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        transform.LookAt(_player.transform);
    }
}

using UnityEngine;

public class Bush : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        _audioSource.Play();
    }
}

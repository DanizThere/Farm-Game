using UnityEngine;

public class BucketWaterbox : MonoBehaviour
{
    private Bucket _bucket;
    private bool _isUsed = false;

    public void Initialize(Bucket bucket)
    {
        _bucket = bucket;
    }

    public void AllowUse(bool allow)
    {
        _isUsed = allow;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isUsed) return;

        if(other.transform.TryGetComponent<Dirt>(out var dirt))
        {
            dirt.TryEffort(_bucket);
        }
    }
}

using UnityEngine;

public class SLMonoBehaviour : MonoBehaviour
{
    private bool _noProblem = true;

    private void Start()
    {
        try
        {
            SLStart();
        }
        catch (System.Exception ex)
        {
            _noProblem = false;
            Debug.LogWarning(ex.Message, gameObject);
        }
    }

    private void Update()
    {
        if (!_noProblem)
        {
            return;
        }

        SLUpdate();
    }

    protected virtual void SLStart()
    {

    }
    protected virtual void SLUpdate()
    {

    }
}

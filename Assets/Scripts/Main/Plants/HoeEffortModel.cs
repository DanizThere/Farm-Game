using UnityEngine;

public class HoeEffortModel : MonoBehaviour
{
    [SerializeField] private Transform _modelTransform;
    private GameObject _model;
    private HoeEffort _effort;

    public void SetModel(GameObject model)
    {
        Destroy(_model);
        _model = null;
        
        if(model != null)
        {
            _model = Instantiate(model, _modelTransform);
            _model.AddComponent<PhysicObject>();
            _model.GetComponent<Rigidbody>().useGravity = false;
            _model.GetComponent<Rigidbody>().isKinematic = true;

            _model.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    public void SetEffort(HoeEffort effort)
    {
        _effort = effort;
    }

    public void Effort()
    {
        if(_model)
            _effort.Effort();
    }
}

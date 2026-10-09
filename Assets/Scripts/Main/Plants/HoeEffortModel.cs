using UnityEngine;

[RequireComponent(typeof(PhysicObject))]
public class HoeEffortModel : EffortModel
{
    private void Awake()
    {
        _physicObject = GetComponent<PhysicObject>();
    }

    public override void SetModel(GameObject model)
    {
        ClearModel();

        if (model != null)
        {
            _model = Instantiate(model, _modelTransform);
            _model.AddComponent<PhysicObject>();
            _model.GetComponent<Rigidbody>().useGravity = false;
            _model.GetComponent<Rigidbody>().isKinematic = true;

            _model.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    public override void ClearModel()
    {
        Destroy(_model);
        _model = null;
    }

    public override void SetEffort(Effort effort)
    {
        _effort = effort;
    }

    public override void Effort()
    {
        if (_model != null)
            _effort.DoEffort();
    }
}

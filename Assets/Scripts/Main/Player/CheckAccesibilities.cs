using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckAccesibilities : MonoBehaviour
{
    [SerializeField] private float _maxRadius = 5f;
    [SerializeField] private GameObject _slappableInteractPrefab;
    [SerializeField] private GameObject _physicObjectInteractPrefab;
    [SerializeField] private Transform _body;

    private Dictionary<PhysicObject, GameObject> _physicInteracts = new();
    private Dictionary<IInteractable, GameObject> _slappableInteracts = new();

    private Transform _canvas3D;

    private void Start()
    {
        _canvas3D = ServiceLocator.Instance.GetService<ViewController>().Canvas3D.transform;
    }

    private void LateUpdate()
    {
        var hits = Physics.SphereCastAll(_body.position, _maxRadius, _body.position);
        if (!hits.Any())
            return;

        var slaps = new List<IInteractable>();
        var physcs = new List<PhysicObject>();

        foreach(var hit in hits)
        {
            var hitTransform = hit.transform;

            if (hitTransform.TryGetComponent<IInteractable>(out var islap))
            {
                if (!_slappableInteracts.ContainsKey(islap))
                {
                    var slapinstance = Instantiate(_slappableInteractPrefab, _canvas3D);
                    slapinstance.GetComponent<InUpdateFollower>().SetTarget(hitTransform);

                    _slappableInteracts.Add(islap, slapinstance);
                }
                slaps.Add(islap);
            }

            if (hitTransform.TryGetComponent<PhysicObject>(out var phys))
            {
                if (!_physicInteracts.ContainsKey(phys))
                {
                    var physinstance = Instantiate(_physicObjectInteractPrefab, _canvas3D);
                    physinstance.GetComponent<InUpdateFollower>().SetTarget(hitTransform);

                    _physicInteracts.Add(phys, physinstance);
                }
                physcs.Add(phys);

            }
        }

        TryRemoveAccess(slaps, physcs);
    }

    private void TryRemoveAccess(List<IInteractable> slaps, List<PhysicObject> physcs)
    {
        var physExcept = _physicInteracts.Keys.Except(physcs).ToList();
        var slapExcept = _slappableInteracts.Keys.Except(slaps).ToList();

        foreach (var physic in physExcept)
        {
            var value = _physicInteracts.GetValueOrDefault(physic);
            Destroy(value);
            _physicInteracts.Remove(physic);
        }
        foreach (var slap in slapExcept)
        {
            var value = _slappableInteracts.GetValueOrDefault(slap);
            Destroy(value);
            _slappableInteracts.Remove(slap);
        }
    }
}

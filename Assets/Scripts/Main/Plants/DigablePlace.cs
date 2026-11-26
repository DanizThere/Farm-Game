using UnityEngine;

public class DigablePlace : MonoBehaviour
{
    public DirtType DirtType;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private GameObject _plane;
    [SerializeField] private Renderer _renderer;
    private Material _material;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, _radius);
    }

    private void Update()
    {
        if (_plane == null || _renderer == null) return;
        if (_material == null)
        {
            _material = _renderer.material;
        }

        var celling = new Vector3
        {
            x = Mathf.RoundToInt(transform.position.x),
            y = transform.position.y,
            z = Mathf.RoundToInt(transform.position.z)
        };

        _material.SetVector("_Center", transform.position);
        _material.SetFloat("_Radius", _radius);

        var scale = _radius / 5;
        _plane.transform.localScale = Vector3.one * scale;
    }

    public void EnableGrid()
    {
        _plane.SetActive(true);
    }

    public void DisableGrid()
    {
        _plane.SetActive(false);
    }

    public bool CanDig(Vector3 point)
    {
        return (point - transform.position).sqrMagnitude <= _radius * _radius;
    }

    
}

using UnityEngine;

public class ThirdDimensionSelectable : MonoBehaviour, ISelectable
{
    private GameObject _cube;
    public void Hide()
    {
        if(_cube != null)
        {
            Destroy(_cube);
            _cube = null;
        }
    }

    public void Show()
    {
        if (_cube != null) return;

        var size = GetComponent<Renderer>().bounds.size + Vector3.one * .3f;

        _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _cube.transform.position = transform.position;

        _cube.name = "Selected Object";

        _cube.transform.localScale = size;

        _cube.GetComponent<Collider>().enabled = false;

        var cubeRenderer = _cube.GetComponent<Renderer>();
        cubeRenderer.material.color = new Color(1,.5f,1,.5f);
    }
}

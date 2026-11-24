using System.Linq;
using UnityEngine;

public class OutlineInteractor : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    private Material _outline;
    private Color _defaultColor;
    private Color _interactColor = Color.white;
    private float _defaultOutline;
    private bool _allowShow = true;

    private void Awake()
    {
        if (!_renderer)
        {
            _renderer = GetComponent<Renderer>();
            if (!_renderer)
            {
                _renderer = GetComponentInChildren<Renderer>();
            }
        }

        var requiredShader = Shader.Find("Shader Graphs/Outline");

        var materials = _renderer.materials;
        _outline = materials.FirstOrDefault(x => x != null && x.shader == requiredShader);

        if (_outline != null)
        {
            _defaultColor = _outline.GetColor("_Color");
            _defaultOutline = _outline.GetFloat("_Outline");
        }
    }
    public void Allow()
    {
        _allowShow = true;
    }

    public void Deny()
    {
        _allowShow = false;
        SetOutline(_defaultColor, 0f);
    }

    public void Show()
    {
        if (_outline == null || !_allowShow) return;
        SetOutline(_interactColor, _defaultOutline);
    }

    public void Hide()
    {
        if (_outline == null) return;
        SetOutline(_defaultColor, _defaultOutline);
    }

    private void SetOutline(Color color, float outline)
    {
        if (_outline == null) return;
        _outline.SetColor("_Color", color);
        _outline.SetFloat("_Outline", outline);

        _renderer.materials = _renderer.materials;
    }
}

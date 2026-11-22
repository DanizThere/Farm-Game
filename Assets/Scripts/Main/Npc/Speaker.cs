using System.Collections;
using System.Linq;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    public bool IsSpeaking;

    private const string MOUTH = "_MouthArrayIndex";

    [SerializeField] private int[] _mouthIndices;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _mouthDuration = .5f;
    private Material _headMaterial;
    private float _currentIndex;

    private float _timer;

    private void Awake()
    {
        var shader = Shader.Find("Shader Graphs/Head");
        _headMaterial = _renderer.materials.FirstOrDefault(x => x.shader == shader);
    }

    private void Update()
    {
        if (IsSpeaking)
        {
            if (_timer <= _mouthDuration)
            {
                _timer += Time.deltaTime;
                return;
            }
            var index = _mouthIndices[Random.Range(0, _mouthIndices.Length)];
            _headMaterial.SetFloat(MOUTH, index);
            _renderer.materials = _renderer.materials;
            _timer = 0;
        }
    }

    [ContextMenu("Stop Talking")]
    public void StopTalking()
    {
        IsSpeaking = false;
        _headMaterial.SetFloat(MOUTH, _currentIndex);
        _renderer.materials = _renderer.materials;
    }

    [ContextMenu("Start Talking")]
    public void StartTalking()
    {
        _currentIndex = _headMaterial.GetFloat(MOUTH);
        IsSpeaking = true;
    }
}

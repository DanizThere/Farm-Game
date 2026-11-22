using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blinker : MonoBehaviour
{
    private const string EYES = "_EyeArrayIndex";

    [SerializeField] private int[] _blinkIndices;
    [SerializeField] private float _blinkDuration = 5f;
    [SerializeField] private Renderer _renderer;
    private Material _headMaterial;
    private float _currentIndex;

    private float _currentDuration;
    private float _timer;

    private void Awake()
    {
        var shader = Shader.Find("Shader Graphs/Head");
        _headMaterial = _renderer.materials.FirstOrDefault(x => x.shader == shader);
        _currentDuration = Random.Range(0, _blinkDuration);
        StartCoroutine(BlinkEnumerator());
    }

    private IEnumerator BlinkEnumerator()
    {
        while (true)
        {
            if (_timer <= _currentDuration)
            {
                _timer += Time.deltaTime;
                yield return null;
                continue;
            }

            _currentIndex = _headMaterial.GetFloat(EYES);
            foreach(var index in _blinkIndices)
            {
                _headMaterial.SetFloat(EYES, index);
                yield return new WaitForSeconds(.1f);
                _renderer.materials = _renderer.materials;
            }

            _headMaterial.SetFloat(EYES, _currentIndex);
            _renderer.materials = _renderer.materials;

            _timer = 0;
            _currentDuration = Random.Range(0, _blinkDuration);
        }
    }
}

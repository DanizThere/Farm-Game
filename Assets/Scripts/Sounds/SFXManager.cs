using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXManager : MonoBehaviour, IService
{
    [SerializeField] private AudioSource _prefab;
    [SerializeField] private List<SFX> _sfxes = new();

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Play(Vector3 position, string id, float volume = 1f)
    {
        var go = Instantiate(_prefab);
        var sfx = _sfxes.FirstOrDefault(x => x.Id == id);
         
        if(sfx != null)
        {
            var clip = sfx.Clips.Random();
            if (clip == null) return;

            StartCoroutine(DestroyObject(go.gameObject, clip.length + .5f));

            go.transform.position = position;
            go.PlayOneShot(clip, volume);
        }
    }

    private IEnumerator DestroyObject(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicManager : MonoBehaviour, IService
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<Music> _musicList = new();
    [SerializeField] private bool _playWithDepends = false;
    private MusicTime _time;
    private TimeManager _timeManager;

    private void Start()
    {
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        _timeManager.OnSunset += () => _time = MusicTime.Night;
        _timeManager.OnSunrise += () => _time = MusicTime.Day;

        //if (_playWithDepends)
        //{
        //    PlayMusicDependsOnDayTime();
        //    return;
        //}

        //PlayMusic();
    }

    public void ToggleMusic()
    {
        if (!_timeManager)
        {
            _timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        }

        _playWithDepends = !_playWithDepends;
        StopAllCoroutines();

        if(_playWithDepends)
        {
            PlayMusicDependsOnDayTime();
            return;
        }

        PlayMusic();
    }


    public void PlayMusic()
    {
        StartCoroutine(PlayMusicEnumerator());
    }

    public void PlayMusicDependsOnDayTime()
    {
        StartCoroutine(PlayMusicDependsOnDayTimeEnumerator());
    }

    private IEnumerator PlayMusicDependsOnDayTimeEnumerator()
    {
        while (true)
        {
            var clips = _musicList.Where(x => x.Time == _time).ToList();
            if (!clips.Any()) continue;
            var clip = clips.Random();
            var length = clip.Mus.length;

            _audioSource.PlayOneShot(clip.Mus);
            yield return new WaitForSeconds(length + 1f);
        }
    }

    private IEnumerator PlayMusicEnumerator()
    {
        while (true)
        {
            var clip = _musicList.Random();
            if (clip == null)
            {
                print("no music");
                break;
            }
            var length = clip.Mus.length;

            _audioSource.PlayOneShot(clip.Mus);
            yield return new WaitForSeconds(length + 1f);
        }
    }
}

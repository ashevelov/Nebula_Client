using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Space;
using Nebula;

public class AudioControloller : Singleton<AudioControloller> {

    public AudioSource _MusicSource;
    public AudioSource _NpcSource;
    public AudioSource _UISource;

    public List<AudioClip> musics = new List<AudioClip>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        foreach(var ac in Resources.LoadAll("Sounds/Music/"))
        {
            musics.Add((AudioClip)ac);
        }
        StartCoroutine(StartMusicSource());
    }

    private IEnumerator StartMusicSource()
    {
        if (musics.Count > 0)
        {
            _MusicSource.clip = musics[Random.Range(0, musics.Count - 1)];
            _MusicSource.Play();
            bool play = true;
            while (play)
            {
                _MusicSource.clip = musics[Random.Range(0, musics.Count)];
                _MusicSource.Play();
                if (_MusicSource.clip != null)
                    yield return new WaitForSeconds(_MusicSource.clip.length);
                else
                {
                    play = false;
                }
            }
        }
    }

    public void PlayNPCVoice(string path)
    {
        if (_NpcSource.isPlaying)
        {
            StopNPCVoice();
        }
        else
        {
            Object ac = Resources.Load(path.inLocation());
            if (ac != null)
            {
                _NpcSource.clip = ac as AudioClip;
                _NpcSource.Play();
                _MusicSource.volume = 0.1f;
            }
            else
            {
                Debug.Log("clip not found: " + path);
            }
        }
    }
    public void StopNPCVoice()
    {
        _NpcSource.Stop();
        _MusicSource.volume = 0.3f;
    }

    public void PlayMusic(string path)
    {
        AudioClip ac = Resources.Load(path.inLocation()) as AudioClip;
        if (ac != null)
        {
            _MusicSource.clip = ac;
        }
    }

    public void PlayUISound(GameSoundsType type)
    {
        if (_UISource != null)
        {
            _UISource.Stop();
            _UISource.clip = DataResources.Instance.GetGameSound(type);
            _UISource.Play();
        }
    }

}

public enum GameSoundsType
{
    action_faled,
    item_focus,
    item_tap,
    module_equip,
    button_click,
    create_module,
    engine_noise,
    work_force_field,
    missile_start,
    missile_flight,
    missile_explosion,
    plasma_start,
    plasma_explosion,
    laser_start,
    laser_hum,
    skill_repair_start,
    skill_repair_hum,
    skill_reinforced_shot,
    buff_positive,
    buff_negative
}

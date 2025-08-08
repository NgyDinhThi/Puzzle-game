using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public Sound[] sounds;

    public const string VolumeKey = "UserVolume";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Tạo AudioSource cho từng sound
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch <= 0 ? 1f : s.pitch;
            s.source.ignoreListenerPause = true;
            s.source.volume = s.volume;
        }

        float saved = PlayerPrefs.GetFloat(VolumeKey, 0.5f);
        SetAllVolumes(saved, save: false);

    }

    public void SetAllVolumes(float value, bool save = true)
    {
        value = Mathf.Clamp01(value);
        foreach (var s in sounds)
        {
            s.volume = value;          // giá trị data
            if (s.source != null)
                s.source.volume = value; // áp dụng vào AudioSource đang chạy
        }

        if (save)
        {
            PlayerPrefs.SetFloat(VolumeKey, value);
            PlayerPrefs.Save();
        }
    }

    public float GetCurrentVolume()
    {
        return PlayerPrefs.GetFloat(VolumeKey, 0.5f);
    }

    private void Start()
    {
        Play("OpenSound");
    }


    public void Play(string name)
    {
       
        Sound s = Array.Find(sounds, sound => sound.name == name);     
        s.source.volume = s.volume;
        s.source.Play();
    }


}

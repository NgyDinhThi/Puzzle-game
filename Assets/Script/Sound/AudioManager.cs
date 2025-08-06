using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public Sound[] sounds;


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

      
    }

    private void Update()
    {
        foreach (Sound s in sounds)
        {
            if (s.source != null)
                s.source.volume = s.volume;
        }
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

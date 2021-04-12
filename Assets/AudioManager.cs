using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume;

    [Range(0.0f, 3.0f)]
    public float pitch;

    public bool loop;
    [HideInInspector] public AudioSource source;

}

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
            }

            return _instance;
        }
    }

    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == soundName);
        s.source.Play();
    }

    public void Pause(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == soundName);
        s.source.Pause();
    }

    public void Resume(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == soundName);
        s.source.UnPause();
    }

}

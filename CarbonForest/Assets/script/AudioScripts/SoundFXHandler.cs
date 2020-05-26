using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Audio;

public class SoundFXHandler : MonoBehaviour {
    public Sound[] sounds;
    public static SoundFXHandler instance;


	// Use this for initialization
	void Awake () {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

		foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void PlayFadeIn(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = 0;
        s.source.Play();
        StartCoroutine(FadeIn(0.1f, s.source));
    }

    public void StopFadeOut(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(FadeOut(0.01f, s.source));
    }

    // --- for fade in/out ---
    public IEnumerator FadeIn(float fadeSpeed, AudioSource source)
    {
        while (source.volume < 1)
        {
            source.volume += fadeSpeed;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator FadeOut(float fadeSpeed, AudioSource source)
    {
        while (source.volume >= 0.05f)
        {
            source.volume -= fadeSpeed;
            yield return new WaitForSeconds(0.01f);
        }
        source.Stop();
    }
    //---
}

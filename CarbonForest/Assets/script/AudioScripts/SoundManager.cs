using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager: MonoBehaviour {
    public AudioSource source;
    bool fadingIn;
    bool fadingOut;
    private void Start()
    {
        StartCoroutine(FadeIn(0.005f));
        source = GetComponent<AudioSource>();
    }

    public IEnumerator FadeIn(float fadeSpeed) {
        while(source.volume < 1)
        {
            source.volume += fadeSpeed;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator FadeOut(float fadeSpeed)
    {
        while(source.volume >= 0.05f)
        {
            source.volume -= fadeSpeed;
            yield return new WaitForSeconds(0.01f);
        }
        source.Stop();
    }
}

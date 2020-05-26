using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDirector : MonoBehaviour
{
    SoundFXHandler soundFXHandler;
    public bool SceneIsInteractive;
    public string[] startMusicNames;
    public string singleBGMname;
    public static MusicDirector instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        soundFXHandler = SoundFXHandler.instance;
        if (SceneIsInteractive)
        {
            foreach (string s in startMusicNames)
            {
                soundFXHandler.Play(s);
            }
        }
        else
        {
            soundFXHandler.Play(singleBGMname);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerEventMusic(string name)
    {
        soundFXHandler.Play(name);
    }

    public void StopAllMusicFadeOut(string[] names)
    {
        foreach (string s in names)
        {
            soundFXHandler.StopFadeOut(s);
        }
    }
}

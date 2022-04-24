using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionHandler : MonoBehaviour {
    public Text gameOverText;
    // Use this for initialization
    public static SceneTransitionHandler instance;

    private int levelIndex;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start () {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
	}
	
	// Update is called once per frame


    public void restartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene()
    {
        try
        {
            if (SceneManager.GetActiveScene().name == "StartMenu")
            {
                if (GameStateHolder.instance.FirstTimePlay == true)
                    SceneManager.LoadScene(levelIndex + 1);
                else
                {
                    SceneManager.LoadScene(Saver.Load().CurrentSceneIndex);
                }
            }
            else
            {
                if (GameStateHolder.instance != null)
                {
                    GameStateHolder.instance.currentSceneIndex =
                        SceneManager.GetActiveScene().buildIndex + 1;
                }

                SceneManager.LoadScene(levelIndex + 1);
            }

        }
        catch (Exception e)
        {
            print(e + ": this is last Scene");
        }
    }

}

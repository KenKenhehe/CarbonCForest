using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFader: MonoBehaviour {
    public static LevelFader instance;

    SoundManager soundManager;
    Animator animator;
    bool canSetScene;
    bool sceneLoading = false;

    private int levelIndex;
    public GameObject objectiveScene;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        soundManager = FindObjectOfType<SoundManager>();
        animator = GetComponent<Animator>();
        canSetScene = (levelIndex == 0 ? true : false);
    }
	
	// Update is called once per frame
	void Update () {
        MenuInput();
	}

    void MenuInput()
    {
        //if (Input.anyKey && canSetScene)
        //{
        //    StartLevelTransition();
        //}
    }

    public void StartLevelTransition()
    {
        animator.SetTrigger("FadeOut");
        StartCoroutine(soundManager.FadeOut(0.01f));
    }

    public void ChangeLevel()
    {
        SceneTransitionHandler.instance.LoadNextScene();
        //try
        //{
        //    if(SceneManager.GetActiveScene().name == "StartMenu")
        //    {
        //        if(GameStateHolder.instance.FirstTimePlay == true)
        //            SceneManager.LoadScene(levelIndex + 1);
        //        else
        //        {
        //            SceneManager.LoadScene(Saver.Load().CurrentSceneIndex);
        //        }
        //    }
        //    else
        //    {
        //        if (GameStateHolder.instance != null)
        //        {
        //            GameStateHolder.instance.currentSceneIndex =
        //                SceneManager.GetActiveScene().buildIndex + 1;
        //        }

        //        SceneManager.LoadScene(levelIndex + 1);
        //    }
            
        //}
        //catch(Exception e)
        //{
        //    print(e + ": this is last Scene");
        //}
    }

    
}

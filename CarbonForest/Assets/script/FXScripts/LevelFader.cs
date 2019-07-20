using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFader: MonoBehaviour {
    SoundManager soundManager;
    Animator animator;
    bool canSetScene;
    bool sceneLoading = false;
    private int levelIndex;
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
        if (Input.GetKeyDown(KeyCode.Space) && canSetScene)
        {
            animator.SetTrigger("FadeOut");
            StartCoroutine(soundManager.FadeOut(0.01f));
        }
    }

    public void ChangeLevel()
    {
        try
        {
            SceneManager.LoadScene(levelIndex + 1);
        }
        catch
        {
            print("this is last Scene");
        }
    }
}

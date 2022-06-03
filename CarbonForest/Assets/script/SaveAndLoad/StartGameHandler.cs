using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameHandler : MonoBehaviour
{
    [Header("Display this UI when this is the first time player")]
    public GameObject FirstStartUI;

    [Header("Normal UI display")]
    public GameObject GameUI;

    public GameObject ConfirmNewGameUI;

    public bool mimicFirstTime = true;

    public Canvas thisCanvas;

    bool canSetScene = false;

    // Start is called before the first frame update
    void Start()
    {
        ConfirmNewGameUI.SetActive(false);
        try
        {
            GameStateHolder.instance.FirstTimePlay = Saver.Load().isFirstTimePlay;
        }
        catch (NullReferenceException e)
        {
            GameStateHolder.instance.FirstTimePlay = true;
        }

        if (GameStateHolder.instance.FirstTimePlay || mimicFirstTime == true)
        {
            FirstStartUI.SetActive(true);
            GameUI.SetActive(false);
            canSetScene = true;
        }
        else
        {
            FirstStartUI.SetActive(false);
            GameUI.SetActive(true);
            canSetScene = false;
        }

        print("Start handler: " + GameStateHolder.instance.FirstTimePlay);
    }

    void Update()
    {
        MenuInput();
    }

    void MenuInput()
    {
        if (Input.anyKey && canSetScene)
        {
            thisCanvas.sortingLayerName = "Default";
            LevelFader.instance.StartLevelTransition();
        }
    }


    public void ShowNewGameConfirm()
    {
        GameUI.SetActive(false);
        ConfirmNewGameUI.SetActive(true);
    }

    public void CancelNewGameConfirm()
    {
        GameUI.SetActive(true);
        ConfirmNewGameUI.SetActive(false);
    }

    public void ContinueGame()
    {
        thisCanvas.sortingLayerName = "Default";
        LevelFader.instance.StartLevelTransition();
    }

    public void StartNewGame()
    {
        thisCanvas.sortingLayerName = "Default";
        GameStateHolder.instance.currentSceneIndex = 1;
        Saver.Save(GameStateHolder.instance);
        LevelFader.instance.StartLevelTransition();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

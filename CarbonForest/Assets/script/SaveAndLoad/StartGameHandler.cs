using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameHandler : MonoBehaviour
{
    [Header("Display this UI when this is the first time player")]
    public GameObject FirstStartUI;

    [Header("Normal UI display")]
    public GameObject GameUI;
    // Start is called before the first frame update
    void Start()
    {
        bool isFirstTime;
        try
        {
            isFirstTime = Saver.Load().isFirstTimePlay;
        }
        catch (Exception)
        {
            isFirstTime = true;
        }

        if (isFirstTime)
        {
            FirstStartUI.SetActive(true);
            GameUI.SetActive(false);
        }
        else
        {
            FirstStartUI.SetActive(false);
            GameUI.SetActive(true);
        }
    }
    private void Update()
    {
       
    }
}

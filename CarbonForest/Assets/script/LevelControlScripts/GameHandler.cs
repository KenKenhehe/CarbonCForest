using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    // Use this for initialization
    public static GameHandler instance;

    [HideInInspector]
    public int globalEnemyCount = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update () {
        if (PauseMenu.GameIsPause == false && TutorialManagerZero.InTutorial == false)
        {
            ResetTime();
        }
	}

   void ResetTime()
    {
        if (Time.timeScale < 1)
        {
            Time.timeScale += 15 * Time.deltaTime;
        }
        else if (Time.timeScale > 1)
        {
            Time.timeScale = 1;
        }
    }
}

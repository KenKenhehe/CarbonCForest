using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct LevelOneState
{

}

struct LevelTwoState
{

}

public class GameStateHolder : MonoBehaviour
{
    public bool FirstTimePlay = true;
    public static GameStateHolder instance;

    [HideInInspector]
    public PlayerGeneralHandler playerData;

    [HideInInspector]
    public int currentSceneIndex = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerGeneralHandler.instance;
    }

}

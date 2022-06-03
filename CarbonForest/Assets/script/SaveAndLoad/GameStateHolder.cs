using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHolder : MonoBehaviour
{
    //------All player state data------
    [HideInInspector]
    public bool FirstTimePlay = true;

    [HideInInspector]
    public int currentSceneIndex = 0;

    [HideInInspector]
    public int weaponCount = 0; // starts from 0

    public static GameStateHolder instance;

    //[HideInInspector]
    //public PlayerGeneralHandler playerData;

   
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    playerData = PlayerGeneralHandler.instance;
    //}

}

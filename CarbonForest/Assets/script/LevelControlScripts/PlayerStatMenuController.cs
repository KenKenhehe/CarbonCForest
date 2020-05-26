using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatMenuController : MonoBehaviour
{
    PlayerGeneralHandler player;
    bool activated = false;
    // Start is called before the first frame update
    private void Awake()
    {
        player = PlayerGeneralHandler.instance;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activated == false)
            return;


    }


}

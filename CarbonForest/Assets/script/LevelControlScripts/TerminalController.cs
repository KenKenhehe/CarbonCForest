using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalController : Interactable
{
    PlayerGeneralHandler player;
    bool controlDeactivated = false;
    public GameObject statusMenu;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerGeneralHandler.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (controlDeactivated == false)
        {
            player.DeactivateControl();
        }
        else
        {
            player.ReactivateControl();
        }
    }
}

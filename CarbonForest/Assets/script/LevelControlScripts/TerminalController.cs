using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalController : Interactable
{
    PlayerGeneralHandler player;
    bool controlDeactivated = false;

    public GameObject SystemUI;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerGeneralHandler.instance;
        if(SystemUI != null)
            SystemUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (SystemUI != null)
        {
            if (controlDeactivated == false)
            {
                player.DeactivateControl();
                //Show UI
                SystemUI.SetActive(true);
            }
            else
            {
                player.ReactivateControl();
                SystemUI.SetActive(false);
            }
        }
    }
}

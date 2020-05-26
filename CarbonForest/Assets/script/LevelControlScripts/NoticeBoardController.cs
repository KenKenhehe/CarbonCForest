using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeBoardController : Interactable
{
    public GameObject[] notices;
    bool hasShowNotice = false;
    PlayerGeneralHandler player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerGeneralHandler.instance;
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
    }

    public override void Interact()
    {
        base.Interact();
        hasShowNotice = !hasShowNotice;
        if(hasShowNotice == true)
        {
            player.DeactivateControl();
        }
        else
        {
            player.ReactivateControl();
        }
    }

    void ShowBoard()
    {
        print("Show board");
    }

    public override void OnClose()
    {
        isClose = true;
    }
}

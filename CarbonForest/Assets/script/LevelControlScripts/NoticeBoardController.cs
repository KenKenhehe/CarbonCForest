using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeBoardController : Interactable
{
    public GameObject[] notices;
    bool isContentShow = false;
    PlayerGeneralHandler player;
    public GameObject boardContent;
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
        isContentShow = !isContentShow;
        if(isContentShow == true)
        {
            player.DeactivateControl();
            ShowBoard(true);
        }
        else
        {
            player.ReactivateControl();
            ShowBoard(false);
        }
    }

    void ShowBoard(bool isShow)
    {
        boardContent.SetActive(isShow);
    }

    public override void OnClose()
    {
        isClose = true;
    }
}

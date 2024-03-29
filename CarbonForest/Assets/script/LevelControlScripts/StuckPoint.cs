﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckPoint : MonoBehaviour
{
    CameraControl camera;
    PlayerGeneralHandler player;
    bool HasTriggered = false;
    // Start is called before the first frame update
    GameHandler gameHandler;
    void Start()
    {
        gameHandler = GameHandler.instance;
        camera = FindObjectOfType<CameraControl>();
        player = FindObjectOfType<PlayerGeneralHandler>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            camera.player = null;
        }
    }
}

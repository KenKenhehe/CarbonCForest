using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCameraHandler : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        if((FindObjectsOfType<Enemy>().Length <= 0 || gameHandler.globalEnemyCount <= 0) && 
            HasTriggered == true)
        {
            camera.player = player.gameObject;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            camera.player = null;
            HasTriggered = true;
        }
    }
}

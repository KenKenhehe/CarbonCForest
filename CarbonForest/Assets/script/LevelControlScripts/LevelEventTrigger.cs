using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventTrigger : MonoBehaviour {
    LevelEnemyEvent enemyEvent;
    bool hasCameralocked;
    public CameraControl camera;
	// Use this for initialization
	void Start () {
        enemyEvent = GetComponent<LevelEnemyEvent>();
        camera = FindObjectOfType<CameraControl>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<LevelEnemyEvent>().DetactFinish();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null || 
            collision.gameObject.GetComponent<MotoController>() != null)
        {
            enemyEvent.StartCoroutine(enemyEvent.SpawnEnemyAtRandomX());
            GetComponent<BoxCollider2D>().enabled = false;
            if(FindObjectsOfType<Pedstrain>().Length > 0)
            {
                foreach(Pedstrain ped in FindObjectsOfType<Pedstrain>())
                {
                    ped.isStardlled = true;
                }
            }
            if (FindObjectOfType<PedastrainHandler>() != null)
            {
                Destroy(FindObjectOfType<PedastrainHandler>().gameObject);
            }
        }

    }
}

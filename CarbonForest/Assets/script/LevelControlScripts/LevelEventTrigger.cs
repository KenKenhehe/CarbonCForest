using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventTrigger : MonoBehaviour {
    LevelEnemyEvent enemyEvent;
	// Use this for initialization
	void Start () {
        enemyEvent = GetComponent<LevelEnemyEvent>();
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
        }
    }
}

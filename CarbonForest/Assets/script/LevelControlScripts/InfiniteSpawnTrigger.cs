using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSpawnTrigger : MonoBehaviour {
    public Enemy[] enemyTypes;
    public int maxEnemyCount = 3;
    LevelEnemyEvent levelEnemyEvent;

    public bool canStopSpawn = false;
    public Transform parent;

    int enemyCount = 0;
	// Use this for initialization
	void Start () {
        levelEnemyEvent = GetComponent<LevelEnemyEvent>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    
    IEnumerator SpawnEnemy()
    {
        while(canStopSpawn == false)
        {
            enemyCount = parent.childCount;
            if (enemyCount < maxEnemyCount)
            {
                GameObject enemyobj =
                    levelEnemyEvent.SpawnEnemyAtRandomXInfinate(enemyTypes[Random.Range(0, 2)].gameObject, parent);   
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            StartCoroutine(SpawnEnemy());
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}

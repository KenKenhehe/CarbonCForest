using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemyEvent : MonoBehaviour {
    public GameObject enemyToSpawn;
    public int enemyAmount = 10;
    public float spawnRate = .1f;
    public float spawnPositionMinX;
    public float spawnPositionMaxX;
    public float yPosition;
    public float offset;
    PlayerGeneralHandler player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerGeneralHandler>();
        this.enabled = false;
        yPosition = transform.position.y - offset;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void DetactFinish()
    {
        if(enemyAmount <= 0)
        {
            Destroy(gameObject);    
        }
    }

    public IEnumerator SpawnEnemyAtRandomX()
    {
        while (enemyAmount > 0)
        {
            GameObject enemyObject = Instantiate(
                enemyToSpawn, 
                new Vector3(Random.Range(spawnPositionMinX, spawnPositionMaxX), yPosition, -1), 
                Quaternion.identity);

            yield return new WaitForSeconds(spawnRate);
            enemyAmount -= 1;
        }
    }

    public GameObject SpawnEnemyAtRandomXInfinate(GameObject enemyToSpawn, Transform parent)
    {
        GameObject enemyObject = Instantiate(
            enemyToSpawn,
            new Vector3(Random.Range(spawnPositionMinX, spawnPositionMaxX), yPosition, -1),
            Quaternion.identity, parent);
        return enemyObject;
    }
}

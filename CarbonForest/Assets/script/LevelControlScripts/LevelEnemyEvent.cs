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
    public float zOffset = -1;
    [Header(" - for upper, + for lower")]
    public float offset;
    [HideInInspector]
    public delegate void ShowEvent();
    [HideInInspector]
    public ShowEvent showEvent;
    PlayerGeneralHandler player;

    [Header("For spawning an enemy at exact location")]
    public Transform spawnTransform;
    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerGeneralHandler>();
        this.enabled = false;
        yPosition = transform.position.y - offset;
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
        if(showEvent != null)
            CallShowEvent();
        while (enemyAmount > 0)
        {
            GameObject enemyObject = Instantiate(
                enemyToSpawn, 
                new Vector3(Random.Range(spawnPositionMinX, spawnPositionMaxX), yPosition, zOffset), 
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

    public void SpawnEnemyAtTransform()
    {
        Instantiate(enemyToSpawn, spawnTransform.position, Quaternion.identity);
    }

    public void CallShowEvent()
    {
        showEvent();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeEnemySpawner : MonoBehaviour
{
    public GameObject BikeEnemyToSpawn;
    public Transform[] SpawnPositions;
    public int enemyAmount = 8;
    public int spawnRate = 1;

    // Update is called once per frame

    public void SpawnBikeEnemyAtRandom()
    {
        if(SpawnPositions.Length > 0)
            StartCoroutine(SpawnEnemyAtRandomX());
    }

    public IEnumerator SpawnEnemyAtRandomX()
    {
        while (enemyAmount > 0)
        {
            Vector3 randomPosition = SpawnPositions[Random.Range(0, SpawnPositions.Length)].position;

            GameObject enemyObject = Instantiate(
                BikeEnemyToSpawn,
                new Vector3(randomPosition.x, randomPosition.y, randomPosition.z),
                Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
            enemyAmount -= 1;
        }
    }
}

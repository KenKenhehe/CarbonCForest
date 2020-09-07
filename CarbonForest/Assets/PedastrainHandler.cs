using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedastrainHandler : MonoBehaviour
{
    public GameObject[] pedastrains;
    public Transform left;
    public Transform right;

    public float pedSpawnOffset;
    public float spawnYPos = -5f;
    // Start is called before the first frame update
    void Start()
    {
        left = GameObject.FindGameObjectWithTag("ColliderLeft").transform;
        right = GameObject.FindGameObjectWithTag("ColliderRight").transform;
        StartCoroutine(SpawnPeople());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnPeople()
    {
        while (true)
        {
            int dir = Random.Range(0, 2) == 1 ? 1 : -1;
            Vector3 sPos = (dir == 1 ? right : left).position;
            GameObject spawnedPed =
                Instantiate(
                pedastrains[Random.Range(0, pedastrains.Length - 1)],
                new Vector3(sPos.x, spawnYPos, Random.Range(-1f, -.1f)) + new Vector3(pedSpawnOffset * dir, 0, 0),
                Quaternion.identity
                );
            spawnedPed.GetComponent<Pedstrain>().moveDir = -dir;
            yield return new WaitForSeconds(3);
        }
    }
}

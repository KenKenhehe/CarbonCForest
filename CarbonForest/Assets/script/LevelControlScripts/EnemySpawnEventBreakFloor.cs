using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnEventBreakFloor : MonoBehaviour
{
    LevelEnemyEvent levelEnemyEvent;
    public GameObject floorIntact;
    public GameObject floorBroken;
    public GameObject floorBreakFX;
    bool hasTriggered = false;
    private void Start()
    {
        floorBroken.SetActive(false);
        floorIntact.SetActive(true);
        levelEnemyEvent = GetComponent<LevelEnemyEvent>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null && hasTriggered == false)
        {
            StartCoroutine(levelEnemyEvent.SpawnEnemyAtTransform());
            
            StartCoroutine(BreakFloor());
            Destroy(gameObject, 1);
            hasTriggered = true;
        }
    }

    IEnumerator BreakFloor()
    {
        yield return new WaitForSeconds(0.3f);
        floorBroken.SetActive(true);
        floorIntact.SetActive(false);
        Instantiate(floorBreakFX, floorBroken.transform.position, Quaternion.identity);
        SoundFXHandler.instance.Play("FloorImpact");
    }
}

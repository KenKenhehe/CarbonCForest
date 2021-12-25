using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParry : MonoBehaviour
{
    IEnumerator AttachParryHintAfterEnemySpawn()
    {
        yield return new WaitForSeconds(2);
        ParryTutorialHandler.instance.AttachParryHintToCurrentEnemy();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            StartCoroutine(AttachParryHintAfterEnemySpawn());
        }
    }
}

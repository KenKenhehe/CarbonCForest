using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public int bossNo = 4;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            if(bossNo == 4)
            {
                SunLeeController.instance.ToCombatMode();
                Destroy(gameObject);
            }
            else if(bossNo == 3)
            {
                LevelThreeBossController.instance.ToCombatMode();
            }
            Destroy(gameObject);
        }
    }
}

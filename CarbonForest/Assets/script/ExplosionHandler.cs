using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    public float explodeRadius = 2;
    public int damage = 5;
    PlayerGeneralHandler player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerGeneralHandler.instance;
        explode();
    }

    void explode()
    {
        FindObjectOfType<ShakeController>().CamShake();
        if (Vector3.Distance(player.transform.position, transform.position) < explodeRadius)
        {
            player.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(damage, 3, null);
        }
        SoundFXHandler.instance.Play("EnemyExplode");
    }
}

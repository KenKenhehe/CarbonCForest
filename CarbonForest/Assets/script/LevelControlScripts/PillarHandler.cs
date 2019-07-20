﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarHandler : MonoBehaviour {

    public int hitCount = 3;
    public GameObject collapseFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BossController>() != null &&
           collision.gameObject.GetComponent<BossController>().dashing == true)
        {
            Instantiate(collapseFX, transform.position,
               collision.gameObject.GetComponent<BossController>().facingRight == true ?
               Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0));
            FindObjectOfType<ShakeController>().CamBigShake();
            hitCount -= 1;
            if (hitCount == 1)
            {
                Destroy(gameObject);
            }
            collision.gameObject.GetComponent<BossController>().stunnedAfterHitPliiar();
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour {
    public float speed = 10;
    bool targetPlayer = true;
    GameObject target;
    Rigidbody2D rb;
    public float rotateSpeed = 10;
    public int Damage = 10;
    public int colorState = 0;
    public GameObject explosionFX;
    SoundFXHandler soundManager;

    public GameObject WhiteStyleFX;
    public GameObject WhiteStyleBreakFX;

    BlockController playerBlockController;
    //Enemy enemy;
    // Use this for initialization
    void Start () {
        WhiteStyleFX.SetActive(false);
        WhiteStyleBreakFX.SetActive(false);
        playerBlockController = PlayerGeneralHandler.instance.gameObject.GetComponent<BlockController>();
        soundManager = SoundFXHandler.instance;
        if (FindObjectOfType<PlayerGeneralHandler>() != null)
        {
            target = FindObjectOfType<PlayerGeneralHandler>().gameObject;
        }
        else
        {
            target = FindObjectOfType<GameObject>().gameObject;
        }
        rb = GetComponent<Rigidbody2D>();
        //enemy = new Enemy();
        Destroy(gameObject, 10);
        
	}
	
	void FixedUpdate () {
        //enemy.facingRight = !(FindObjectOfType<PlayerMovement>().facingRight);
		if(target != null)
        {
            HeadTowardTarget();
        }
        if (WhiteStyleFX != null)
        {
            if (playerBlockController.blocking == true)
            {
                WhiteStyleFX.SetActive(true);
            }
            else
            {
                WhiteStyleFX.SetActive(false);
            }
        }
	}

    void HeadTowardTarget()
    {
        Vector2 direction = (Vector2)target.transform.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed * Time.fixedDeltaTime;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BossController>() == null && targetPlayer == true)
        {
            if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
            {
                GameObject player = collision.gameObject;
                player.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(Damage, 0, FindObjectOfType<BossController>());
                if(player.GetComponent<BlockController>().blocking == true && 
                    player.GetComponent<PlayerMovement>().facingRight != FindObjectOfType<BossController>().facingRight)
                {
                    soundManager.Play("SwordCling1");
                    FindObjectOfType<ShakeController>().CamShake();
                    if (target != null)
                    {
                        RetargetToBoss();
                    }
                }
                else
                {
                    MissileExplode();
                }
            }
            else if(collision.gameObject.GetComponent<PillarHandler>() == null && 
                collision.gameObject.GetComponent<SceneSwitchHandler>() == null)
            {
                MissileExplode();
            }
        }
       
        if(collision.gameObject.GetComponent<BossController>() != null && targetPlayer == false)
        {
            MissileExplode();
            BossController boss = collision.gameObject.GetComponent<BossController>();
            DamageBoss(boss, Damage);
        }
    }

    void MissileExplode()
    {
        SoundFXHandler.instance.Play("MissileExplode");
        FindObjectOfType<ShakeController>().CamBigShake();
        Instantiate(explosionFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void DamageBoss(BossController boss, int damage)
    {
        boss.TakeDamage(damage);
    }

    void RetargetToBoss()
    {
        target = FindObjectOfType<BossController>().gameObject;
        
        targetPlayer = false;
        rotateSpeed += Random.Range(1000, 2000);
        Destroy(WhiteStyleFX);

        WhiteStyleBreakFX.SetActive(true);
        WhiteStyleBreakFX.transform.parent = null;
    }

    private void OnDestroy()
    {
        MissileExplode();
    }
}

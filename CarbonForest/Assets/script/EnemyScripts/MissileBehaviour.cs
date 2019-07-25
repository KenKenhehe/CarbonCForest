using System.Collections;
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
    //Enemy enemy;
	// Use this for initialization
	void Start () {
        target = FindObjectOfType<PlayerGeneralHandler>().gameObject;
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
    }

    private void OnDestroy()
    {
        MissileExplode();
    }
}

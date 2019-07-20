using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public PlayerMovement target;
    public float speed;
    Rigidbody2D rb2d;
    public EnemyShooterController enemy = new EnemyShooterController();
    public int damage = 10;
    Vector2 moveDirection;

	// Use this for initialization
	void Start () {
        target = FindObjectOfType<PlayerMovement>();
        rb2d = GetComponent<Rigidbody2D>();
        moveDirection = new Vector2(
            (target.transform.position.x - transform.position.x > 0 ? 1 : -1) * speed * Time.deltaTime, 
            0);
        rb2d.velocity = moveDirection;
        Destroy(gameObject, 3f);
        enemy.facingRight = rb2d.velocity.x > 0 ? true : false;
	}
	
	// Update is called once per frame
	void Update () {
       
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            DetactMissHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerAttack>() != null)
        {
            GameObject player = collision.gameObject;
            if (player.GetComponent<BlockController>().blocking == false && 
                player.GetComponent<PlayerMovement>().dodging == false)
            {
                player.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(damage, 0, enemy);
                Destroy(gameObject);
            }
            else if(player.GetComponent<BlockController>().blocking == true)
            {
                FindObjectOfType<SoundFXHandler>().Play("SwordCling1");
                player.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(damage, 0, enemy);
                rb2d.velocity = -moveDirection;
                rb2d.velocity = new Vector2(rb2d.velocity.x * 1.5f, Random.Range(-10f, 10f));
            }
        }
    }

    void DetactMissHit()
    {
        if(Vector2.Distance(transform.position, target.transform.position) <= 1f)
        {
            if(target.GetComponent<PlayerMovement>() != null && 
                target.GetComponent<PlayerMovement>().dodging == true)
            {
                Time.timeScale = .001f;
            }
        }
    }
}

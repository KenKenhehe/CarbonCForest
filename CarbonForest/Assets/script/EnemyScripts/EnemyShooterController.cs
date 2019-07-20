using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController : Enemy {
    public GameObject bulletPref;
    public Vector2 deamageFXOffset = Vector2.zero;

    //set to random on instantiate.
    protected float respondRange;
    protected float fireRate;

    protected float fireTime = 0;
    protected Animator animator;
    ShakeController shakeController;
    SpriteRenderer renderer;
    protected Rigidbody2D rb2d;
    WaitForSeconds hitDuration = new WaitForSeconds(0.05f);
    SceneEventHandler sceneEventHandler;
    // Use this for initialization

    void Start () {
        Initialize();
    }

    public void Initialize()
    {
        sceneEventHandler = FindObjectOfType<SceneEventHandler>();
        playerToFocus = FindObjectOfType<PlayerAttack>();
        fireRate = Random.Range(1.0f, 3.0f);
        health = Random.Range(minHealth, maxHealth);
        startHealth = health;
        respondRange = Random.Range(3.5f, 8);
        renderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        shakeController = FindObjectOfType<ShakeController>();
        facingRight = false;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        healthBar.fillAmount = health / startHealth;
    }

    // Update is called once per frame
    void Update () {
        if (playerToFocus != null)
        {
            MoveTowardsPlayer();
            FacePlayer();
            AttackPlayer();
        }
	}

    void OnlyMoveBetween(float minX, float maxX)
    {
        if (transform.position.x >= maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }

    }

    public override void AttackPlayer()
    {
        base.AttackPlayer();
        fireTime += Time.deltaTime;
        if (fireTime >= fireRate && takingDamage == false)
        {
            Instantiate(bulletPref, transform.position, Quaternion.identity);
            FindObjectOfType<SoundFXHandler>().Play("LaunchBullet");
            fireTime = 0;
        }
    }


    public override void  TakeDamage(int damage)
    {
        FindObjectOfType<SoundFXHandler>().Play("DamageSmall");
        base.TakeDamage(damage);
        GameObject bloodfX = Instantiate<GameObject>(bloodFX, transform);
        bloodfX.transform.Rotate(0, facingRight ? 0 : 180, 0);
        fireTime = 0;
        animator.SetTrigger("Damaged");
        if (damage < 3)
        {
            shakeController.CamShake();
            Time.timeScale = Random.Range(0.15f, 0.3f);
        }
        else if(damage >= 3)
        {
            shakeController.CamBigShake();
            Time.timeScale = Random.Range(0.05f, 0.2f);
        }
        
        if (health <= 1)
        {
            PlayExplosionSound();
            shakeController.CamBigShake();
            Instantiate(destoryFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
            if (explosionFXs != null)
            {
                Instantiate(explosionFXs[Random.Range(0, explosionFXs.Length)], transform.position, Quaternion.Euler(
                    transform.rotation.x, 
                    transform.rotation.y,
                    Random.Range(-20, 20))
                    );
            }
        }
        
    }

    void MoveTowardsPlayer()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.deltaTime;
        }

        if (playerToFocus.transform.position.x - respondRange > transform.position.x)
        {
            rb2d.velocity = new Vector2(speed * Time.deltaTime, rb2d.velocity.y);

        }
        else if(playerToFocus.transform.position.x + respondRange < transform.position.x)
        {
            rb2d.velocity = new Vector2(-(speed * Time.deltaTime), rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);
        }

        if(rb2d.velocity != Vector2.zero)
        {
            animator.SetBool("IsWalking", true);
        }

    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    public override float GetHealth()
    {
        return base.GetHealth();
    }
}

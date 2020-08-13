using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController : Enemy {
    public GameObject bulletPref;
    public Vector2 deamageFXOffset = Vector2.zero;

    public float minRespondRange = 3.5f;
    public float maxRespondRange = 8;

    //set to random on instantiate.
    protected float fireRate;

    protected float fireTime = 0;
    ShakeController shakeController;
    SpriteRenderer renderer;
    WaitForSeconds hitDuration;
    SceneEventHandler sceneEventHandler;
    // Use this for initialization

    void Start () {
        Initialize();
    }

    public void Initialize()
    {
        hitDuration = new WaitForSeconds(0.05f);
        sceneEventHandler = FindObjectOfType<SceneEventHandler>();
        playerToFocus = FindObjectOfType<PlayerAttack>();
        fireRate = Random.Range(1.0f, 3.0f);
        health = Random.Range(minHealth, maxHealth);
        startHealth = health;
        respondRange = Random.Range(minRespondRange, maxRespondRange);
        renderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        shakeController = FindObjectOfType<ShakeController>();
        facingRight = false;
        animator = GetComponent<Animator>();
        soundFXHandler = SoundFXHandler.instance;
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
            MoveToPlayer();
            FacePlayer();
            AttackPlayer();
            PlayDynamicAnimation();
        }
	}

    public override void AttackPlayer()
    {
        base.AttackPlayer();
        fireTime += Time.deltaTime;
        if (fireTime >= fireRate && takingDamage == false)
        {
            GameObject bullet = Instantiate(bulletPref, transform.position, Quaternion.identity);
            bullet.GetComponent<BulletController>().enemy = this;
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

    public void setHealth(int health)
    {
        this.health = health;
    }

    public override float GetHealth()
    {
        return base.GetHealth();
    }
}

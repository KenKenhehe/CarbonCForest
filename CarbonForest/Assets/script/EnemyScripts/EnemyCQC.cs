using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCQC : Enemy {
    protected ShakeController shakeController;
    protected SoundFXHandler soundFXHandler;
    public bool withinAttackRange = false;
    public int blockDamageMultiplyer = 6;
    public GameObject blockBlob;
    public GameObject blockExplosionFX;
    public GameObject parryBoomFX;
    public Color blockWhite;
    public Color blockBlue;

    public Image blockBar;

    protected  bool canAttack = true;

    protected float randHoldTime;

    public int damage;
    public float shockForce = .5f;

    protected Rigidbody2D rb2d;
    protected float respondRange = 0;
    protected float attackRate;
    protected Animator animator;
    protected SpriteRenderer renderer;
    public float attackRange = 1.5f;
    public float stunnedDuration = 4f;
    protected int colorState;

    protected float randomPatrolDir;
    // Use this for initialization
    void Start () {
        Initialize();
    }
	
	// Update is called once per frame
	void Update ()
    {
        EnableBehaviour();
    }

    public virtual void Initialize()
    {
        hitDuration = new WaitForSeconds(0.05f);
        randomPatrolDir = Random.Range(-1f, 1f);
        animator = GetComponent<Animator>() == null ? GetComponentInChildren<Animator>() : GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>() == null ? GetComponentInChildren<SpriteRenderer>() : GetComponent<SpriteRenderer>();
        shakeController = FindObjectOfType<ShakeController>();
        soundFXHandler = FindObjectOfType<SoundFXHandler>();
        health = Random.Range(minHealth, maxHealth);
        startHealth = health;
        playerToFocus = FindObjectOfType<PlayerAttack>();
        respondRange = Random.Range(1f, 1.5f);
        SightRange = Random.Range(minSightRange, maxSightRange);
        rb2d = GetComponent<Rigidbody2D>();
        randHoldTime = Random.Range(.1f, 3f);
        attackRate = Random.Range(.3f, 1.5f);
        colorState = Random.Range(0, 2);
        blockColorRenderer = blockBlob.GetComponent<SpriteRenderer>();

        blockColorRenderer.color = new Color(blockColorRenderer.color.r,
            blockColorRenderer.color.g, blockColorRenderer.color.b,
            0);

        if(blockBar != null)
        {
            blockBar.fillAmount = blockPoint / maxBlockPoint;
        }
        StartCoroutine(ChangePatrolDir());
        ChangeBlockColorAtRandom();
    }

    public virtual void EnableBehaviour()
    {
        if (playerToFocus != null)
        {
            if (canAttack == true && canMove == true)
                FacePlayer();

            MoveToPlayerWithRandomRate();

            AttackPlayerAtRate();
        }
    }

    public void MoveToPlayerWithRandomRate()
    {
        if (randHoldTime <= 0 && canAttack == true && canMove == true)
        {
            MoveToPlayer();
        }
        else
        {
            randHoldTime -= Time.fixedDeltaTime;
            if (canMove == true)
                rb2d.velocity = new Vector2(speed * randomPatrolDir * Time.fixedDeltaTime, rb2d.velocity.y);
            else
                rb2d.velocity = Vector2.zero;
        }
    }

    public void AttackPlayerAtRate()
    {
        if (attackRate <= 0)
        {
            AttackPlayer();
            attackRate = Random.Range(.3f, 1.5f);
        }
        else
        {
            attackRate -= Time.deltaTime;
        }
    }

    IEnumerator ChangePatrolDir()
    {
        while (true)
        {
            if (PlayerSeen)
            {

            }
            randomPatrolDir = Random.Range(0, 2) == 0 ? -1 : 1;
            yield return new WaitForSeconds(.5f);
        }
    }

    public virtual void ApplyDamage()
    {
        FindObjectOfType<SoundFXHandler>().Play("EnemySwordSwing");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + (new Vector3(1,0,0) * (facingRight ? 1 : -1)), attackRange);
        foreach(Collider2D collider in colliders)
        {
            if(collider.GetComponent<PlayerGeneralHandler>() != null)
            {
                collider.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(damage,colorState, this);
                collider.transform.position = new Vector3(
                (facingRight == true ? collider.transform.position.x + shockForce : collider.transform.position.x - shockForce),
                collider.transform.position.y,
                collider.transform.position.z);
                shakeController.CamShake();
                if (colorState == collider.GetComponent<PlayerGeneralHandler>().colorState &&
                    collider.GetComponent<BlockController>().blocking == true && 
                    collider.GetComponent<PlayerMovement>().facingRight != facingRight)
                {
                    blockPoint -= 1;
                    if (blockBar != null)
                    {
                        blockBar.fillAmount = (float)blockPoint / maxBlockPoint;
                    }
                    soundFXHandler.Play("SwordCling" + Random.Range(1, 5));
                }
            
                if(blockPoint <= 0)
                {
                    if(parryBoomFX != null)
                    {
                        GameObject pbFX = Instantiate(parryBoomFX, transform.position, Quaternion.identity);
                    }
                    soundFXHandler.Play("ParrySuccess1");
                    StartCoroutine(DisableAttackForAWhile(stunnedDuration));

                    shakeController.CamBigShake();
                    Time.timeScale = .001f;
                    //collider.GetComponent<PlayerGeneralHandler>().CallPowerUp();
                    health -= damage * blockDamageMultiplyer;
                    animator.SetTrigger("Stunned");
                    if (healthBar != null)
                    {
                        healthBar.fillAmount = (float)health / startHealth;
                    }
                    if (maxHealth < 20 ) // drones max health must be less than 20
                    {
                        rb2d.AddForce(facingRight == true ? 
                            new Vector2(-9000, Random.Range(10000, 12000)): 
                            new Vector2(9000, Random.Range(10000, 12000)));
                        rb2d.gravityScale = 70;
                    }
                    else if(maxHealth > 200)
                    {
                        Instantiate(blockExplosionFX, transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }

    public override void AttackPlayer()
    {
        if (canAttack == true)
        {
            base.AttackPlayer();
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange + 1)
            {
                animator.SetTrigger("Attack1");
                animator.SetTrigger("Attack2");
                randHoldTime = Random.Range(.1f, 3f);
            }
        }
    }

    public void TakeDamageDeath()
    {
        TakeDamage(10);
    }

   public override void TakeDamage(int damage)
    {
        FindObjectOfType<SoundFXHandler>().Play("DamageSmall");
        animator.SetTrigger("Damaged");
        base.TakeDamage(damage);
        GameObject bloodfX = Instantiate<GameObject>(bloodFX, transform);
        bloodfX.transform.Rotate(0, facingRight ? 0 : 180, 0);
        attackRate = Random.Range(1f, 2f);
        StartCoroutine(TakeDamageForAWhile());
        if (damage < 3)
        {
            shakeController.CamShake();
            Time.timeScale = Random.Range(0.15f, 0.3f);
        }
        else if (damage >= 3)
        {
            shakeController.CamBigShake();
            Time.timeScale = Random.Range(0.05f, 0.2f);
        }

        if (health <= 1)
        {
            DeathBehaviour();
            PlayExplosionSound();
        }
    }

    public override void DeathBehaviour()
    {
        base.DeathBehaviour();
        shakeController.CamBigShake();
        Instantiate(destoryFX, transform.position, Quaternion.identity);
        if (explosionFXs != null)
        {
            Instantiate(explosionFXs[Random.Range(0, explosionFXs.Length)], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public virtual void MoveToPlayer()
    {        
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.deltaTime;
        }

        if (playerToFocus.transform.position.x - respondRange > transform.position.x)
        {
            rb2d.velocity = new Vector2(speed * Time.fixedDeltaTime, rb2d.velocity.y);
        }
        else if (playerToFocus.transform.position.x + respondRange < transform.position.x)
        {
            rb2d.velocity = new Vector2(-(speed * Time.fixedDeltaTime), rb2d.velocity.y); 
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    public void ChangeBlockColorAtRandom()
    {
        if(colorState == 0)
        {
            blockColorRenderer.color = blockWhite;
        }
        else if(colorState == 1)
        {
            blockColorRenderer.color = blockBlue;
        }
    }

    public IEnumerator DisableAttackForAWhile(float duration)
    {
        //renderer.color = new Color(1, 0.5f, 0.5f, 1);
        animator.SetBool("StunnedIdle", true);
        canAttack = false;
        canMove = false;
        yield return new WaitForSeconds(duration);
        canAttack = true;
        canMove = true;
        blockPoint = maxBlockPoint;
        animator.SetBool("StunnedIdle", false);
        if (blockBar != null)
        {
            blockBar.fillAmount = (float)blockPoint / maxBlockPoint;
        }
        //renderer.color = Color.white;
    }

    public IEnumerator TakeDamageForAWhile()
    {
        canAttack = false;
        yield return new WaitForSeconds(2);
        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyCQC>() != null)
        {
            collision.gameObject.GetComponent<EnemyCQC>().rb2d.velocity  = 
                new Vector2(speed * Time.deltaTime * Random.Range(0, 2), rb2d.velocity.y);
        }
    }

    public override float GetHealth()
    {
        return base.GetHealth();
    }

    public void Patrol()
    {

    }
}

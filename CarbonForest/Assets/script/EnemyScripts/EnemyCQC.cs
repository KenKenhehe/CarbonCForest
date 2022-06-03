using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCQC : Enemy
{
    //protected ShakeController shakeController;
    public bool withinAttackRange = false;
    [Range(0, 100)]
    [Header("Percentage of this enemy movement disabled when taken damage")]
    public int hitRecoverRate = 50;
    public int blockDamageMultiplyer = 6;
    public GameObject blockBlob;
    public GameObject blockExplosionFX;

    public GameObject parryBoomFX;

    public GameObject standChangeFXNeg;
    public GameObject standChangeFXPos;
    public Vector3 standChangeFXOffset;


    public Color blockWhite;
    public Color blockBlue;

    public float minAttackRate = .3f;
    public float maxAttackRate = 1.5f;

    public Image blockBar;

    protected bool canAttack = true;
    protected bool IsStunned = false;

    protected float randHoldTime;

    public int damage;
    public float shockForce = .5f;

    protected float attackRate;
    //protected Animator animator;
    protected SpriteRenderer renderer;
    public float attackRange = 1.5f;
    public float stunnedDuration = 4f;
    protected int colorState;
    protected int currentColorState;

    protected float randomPatrolDir;
    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsStunned == false)
            EnableBehaviour();
    }

    public virtual void Initialize()
    {
        GameHandler.instance.globalEnemyCount += 1;
        if (AllStatusBars != null)
            AllStatusBars.SetActive(false);
        
        hitDuration = new WaitForSeconds(0.2f);
        randomPatrolDir = Random.Range(-1f, 1f);
        animator = GetComponent<Animator>() == null ? GetComponentInChildren<Animator>() : GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>() == null ? GetComponentInChildren<SpriteRenderer>() : GetComponent<SpriteRenderer>();
        shakeController = ShakeController.instance;
        soundFXHandler = SoundFXHandler.instance;
        health = Random.Range(minHealth, maxHealth);
        startHealth = health;
        playerToFocus = FindObjectOfType<PlayerAttack>();
        respondRange = Random.Range(1f, 1.5f);
        SightRange = Random.Range(minSightRange, maxSightRange);
        rb2d = GetComponent<Rigidbody2D>();
        //rb2d.isKinematic = true;
        //rb2d.useFullKinematicContacts = true;
        randHoldTime = Random.Range(.1f, 3f);
        attackRate = Random.Range(minAttackRate, maxAttackRate);
        colorState = Random.Range(0, 2);
        currentColorState = colorState;
        blockColorRenderer = blockBlob.GetComponent<SpriteRenderer>();
        

        blockColorRenderer.color = new Color(blockColorRenderer.color.r,
            blockColorRenderer.color.g, blockColorRenderer.color.b,
            0);

        if (blockBar != null)
        {
            blockBar.fillAmount = blockPoint / maxBlockPoint;
        }
        //StartCoroutine(ChangePatrolDir());
        ChangeBlockColorAtRandom();
        CanMoveAfterShowAnimation();
        
    }

    public virtual void EnableBehaviour()
    {
        PlayDynamicAnimation();
        if (playerToFocus != null && PlayerGeneralHandler.instance.isDead == false)
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
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }

    

    public void AttackPlayerAtRate()
    {
        if (attackRate <= 0)
        {
            AttackPlayer();
            attackRate = Random.Range(minAttackRate, maxAttackRate);
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
        PlaySlashSound();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + (new Vector3(1, 0, 0) * (facingRight ? 1 : -1)), attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<PlayerGeneralHandler>() != null)
            {
                collider.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(damage, colorState, this);

                //Prevent player goes out of screen 
                if (collider.GetComponent<PlayerGeneralHandler>().hasCollideWithEdge == false)
                {
                    collider.transform.position = new Vector3(
                    (facingRight == true ? collider.transform.position.x + shockForce : collider.transform.position.x - shockForce),
                    collider.transform.position.y,
                    collider.transform.position.z);
                }

                shakeController.CamShake();
                if (colorState == collider.GetComponent<PlayerGeneralHandler>().colorState &&
                    collider.GetComponent<BlockController>().blocking == true &&
                    collider.GetComponent<PlayerMovement>().facingRight != facingRight)
                {
                    if (AllStatusBars != null)
                        AllStatusBars.SetActive(true);
                    AddiBlockEvent();
                    blockPoint -= playerToFocus.currentWeapon.parryPoint;
                    if (blockBar != null)
                    {
                        blockBar.fillAmount = (float)blockPoint / maxBlockPoint;
                    }
                    soundFXHandler.Play("SwordCling" + Random.Range(1, 5));
                }

                if (blockPoint <= 0)
                {
                    playerToFocus.GetComponent<PlayerGeneralHandler>().IncreaseFlowPoint();
                    if (parryBoomFX != null)
                    {
                        GameObject pbFX = Instantiate(parryBoomFX, transform.position, Quaternion.identity);
                    }
                    soundFXHandler.Play("ParrySuccess1");
                    ParriedBehaviour();
                    StartCoroutine(DisableAttackForAWhile(stunnedDuration));

                    shakeController.CamBigShake();
                    Time.timeScale = .002f;
                    health -= damage * blockDamageMultiplyer;
                    animator.SetTrigger("Stunned");
                    UpdateHealthUI();
                    if (maxHealth < 20) // drones max health must be less than 20
                    {
                        rb2d.AddForce(facingRight == true ?
                            new Vector2(-2000, Random.Range(3500, 4000)) :
                            new Vector2(2000, Random.Range(3500, 4000)));
                        rb2d.gravityScale = 60;
                    }
                    else if (maxHealth > 200)
                    {
                        float FXRotation = facingRight ? 90 : -90;
                        Instantiate(blockExplosionFX, transform.position - new Vector3(0, 1, 0), Quaternion.Euler(0, 0, FXRotation));
                    }
                }
            }
        }
    }

    public virtual void AddiBlockEvent()
    {

    }

    public override void AttackPlayer()
    {
        if (canAttack == true)
        {
            base.AttackPlayer();
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange + 1)
            {
                canMove = false;
                animator.SetTrigger("Attack1");
                animator.SetTrigger("Attack2");
                randHoldTime = Random.Range(.1f, 3f);
            }
            else
            {
                canMove = true;
            }
        }
    }

    public void SpawnChangeStandFX(bool positron)
    {
        print("spawned stand fx");
        Instantiate(positron == true ? standChangeFXPos : standChangeFXNeg, 
            transform.position + standChangeFXOffset, Quaternion.identity);
    }

    public void TakeDamageDeath()
    {
        TakeDamage(10);
    }

    public override void PlayExplosionSound()
    {
        soundFXHandler.Play("ExplodeMech");
    }

    public override void TakeDamage(int damage)
    {
        blockBar.enabled = true;
        PlayTakeDamageSound();
        
        base.TakeDamage(damage);
        if (health <= 1)
        {
            DeathBehaviour();
            PlayExplosionSound();
        }
        else
        {
            int bloodObjIndex = Random.Range(0, bloodFX.Length);
            int spawnDir = (facingRight == true ? 1 : -1);
            Vector3 finalOffset = new Vector3(BloodFXOffset.x * spawnDir, BloodFXOffset.y, BloodFXOffset.z);
            GameObject bloodfX = Instantiate(bloodFX[bloodObjIndex], transform.position + finalOffset, transform.rotation);

            //int spawnDir = (facingRight == true? -1 : 1);

            bloodfX.transform.localScale = new Vector3(
                bloodfX.transform.localScale.x * spawnDir,
                bloodfX.transform.localScale.y,
                bloodfX.transform.localScale.z
                );

            StartCoroutine(DamagedEffect());
            int chance = Random.Range(0, 100);
            bool enterHitRecover = chance <= hitRecoverRate ? true : false;
            if (enterHitRecover)
            {
                StartCoroutine(TakeDamageForAWhile());

                animator.SetTrigger("Damaged");
            }
        }
    }

    public override void DeathBehaviour()
    {
        base.DeathBehaviour();
        playerToFocus.GetComponent<PlayerGeneralHandler>().RestoreHealth();
        shakeController.CamBigShake();
        Instantiate(destoryFX, transform.position + DeathFXOffset, Quaternion.identity);
        if (explosionFXs != null)
        {
            Instantiate(explosionFXs[Random.Range(0, explosionFXs.Length)], transform.position + DeathFXOffset, Quaternion.identity);
        }
        Destroy(gameObject);
    }



    public void ChangeBlockColorAtRandom()
    {
        if (colorState == 0)
        {
            blockColorRenderer.color = blockWhite;
        }
        else if (colorState == 1)
        {
            blockColorRenderer.color = blockBlue;
        }
        currentColorState = colorState;
    }

    public IEnumerator DisableAttackForAWhile(float duration)
    {
        //renderer.color = new Color(1, 0.5f, 0.5f, 1);
        IsStunned = true;
        canAttack = false;
        canMove = false;
        animator.SetBool("StunnedIdle", true);        
        yield return new WaitForSeconds(duration);
        canAttack = true;
        canMove = true;
        IsStunned = false;
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
        // For all boss or elite enemy, attack continues
        if (maxHealth < 100)
        {
            attackRate = Random.Range(1f, 2f);
            canAttack = false;
            yield return new WaitForSeconds(2);
            canAttack = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyCQC>() != null)
        {
            collision.gameObject.GetComponent<EnemyCQC>().rb2d.velocity =
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

    public virtual void ParriedBehaviour()
    {

    }

    public override void ShowEnemyCurrentStand(bool show)
    {
        //Instantiate stand icon with animation
        if (WhiteStandFX != null && BlueStandFX != null)
        {
            if (show == true)
            {
                if (colorState == 0)
                {
                    WhiteStandFX.SetActive(true);
                    BlueStandFX.SetActive(false);
                }
                else if (colorState == 1)
                {
                    WhiteStandFX.SetActive(false);
                    BlueStandFX.SetActive(true);
                }
            }
            else
            {
                WhiteStandFX.SetActive(false);
                BlueStandFX.SetActive(false);
            }
        }
    }

    public void SetColorState(int color)
    {
        colorState = color;
    }

    public override void PlaySlashSound()
    {
        soundFXHandler.Play("PoliceAttack" + Random.Range(1,4));
    }

    public int GetColorState()
    {
        return colorState;
    }

    public void playShowBurstFX()
    {
        soundFXHandler.Play("Burst");
    }

    public void playHitGroundFX()
    {
        shakeController.CamShake();
        soundFXHandler.Play("PillarHit");
    }

    public void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)health / startHealth;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunleeController : EnemyCQC
{
    bool isOnBike = true;
    bool FallFromBike = true;
    bool isStunned = false;
    bool isOffBikeMode = false;

    bool isShootMode = false;

    public RuntimeAnimatorController sunleeAnimator;
    public static SunleeController instance;
    public SunLeeBikeController bikeController;

    public Dialog outroDialog;

    [HideInInspector]
    public int fallDir = 1;

    bool falling;
    float currentFallingTime;
    public float FallTime;
    public float fallSpeed;
    public float stunTime = 3;

    public float frictionSpeed = 150;
    public float WalkSpeed = 1000;
    public float DashSpeed = 1500;

    public int projectileGap = 8;

    public GameObject ChangeToNegFX;
    public GameObject ChangeToPosFX;

    public GameObject Beam;

    public GameObject HealthBarFrameToDisable;

    int meleeAttackCount = 0;

    //DEBUG
    bool hasTakenExplosionDamage = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        currentFallingTime = FallTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        Initialize();
        animator.runtimeAnimatorController = sunleeAnimator;
        blockBar.gameObject.transform.parent.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (isOffBikeMode)
        {
            if (isShootMode)
            {
                EnableShootBehaviour();
            }
            else
            {
                EnableBehaviour();
            }
        }
        //TODO: if sunlee is 
        else if (FallFromBike == true && isStunned == false)
        {
            //walk to bike
            MoveToBike();
        }
        else
        {
            //EnableBehaviour();
        }
    }

    void EnableShootBehaviour()
    {
        animator.SetTrigger("Shoot");
        meleeAttackCount = 0;
        isShootMode = false;

    }

    public override void EnableBehaviour()
    {
        PlayDynamicAnimation();
        if (playerToFocus != null && 
            PlayerGeneralHandler.instance.isDead == false && 
            isStunned == false)
        {
            print("In attack mode");
            if (canAttack == true && canMove == true)
                FacePlayer();

            if (attacking)
                MoveToPlayer();
            else
                MoveToPlayerWithRandomRate();

            if (playerToFocus.GetComponent<PlayerGeneralHandler>().IsStunned)
            {
                FollowUpAttack();
            }
            else
                AttackPlayerAtRate();
        }

        if(meleeAttackCount >= Random.Range(3, 5))
        {
            canMove = false;
            isShootMode = true;
        }
    }

    public void FollowUpAttack()
    {
        MoveToPlayer();
        if (attacking)
        {
            animator.SetTrigger("FollowUpAttack");
            attacking = false;
        }
    }

    public override void AttackPlayer()
    {
        print("Sunlee attack");
        if (canAttack == true)
        {
            base.AttackPlayer();
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange + 1 &&
                attacking == false)
            {
                canMove = false;
                animator.SetTrigger("Attack");
                //If already attacking don't set this trigger again
                attacking = true;
                meleeAttackCount += 1;
            }
            else
            {
                canMove = true;
            }
        }
    }

    public void Dash()
    {

    }



    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    void MoveToBike()
    {
        animator.SetBool("IsWalking", true);
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.deltaTime;
        }

        if (bikeController.transform.position.x - respondRange > transform.position.x)
        {
            rb2d.velocity = new Vector2(speed * Time.fixedDeltaTime, rb2d.velocity.y);
        }
        else if (bikeController.transform.position.x + respondRange < transform.position.x)
        {
            rb2d.velocity = new Vector2(-(speed * Time.fixedDeltaTime), rb2d.velocity.y);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            rb2d.velocity = Vector2.zero;
        }
    }

    void Fall()
    {
        if (falling == true)
        {
            transform.localScale = new Vector2(xSize * -bikeController.currentDir, transform.localScale.y);
            if (currentFallingTime <= 0)
            {
                falling = false;
                currentFallingTime = FallTime;
                rb2d.velocity = Vector2.zero;
            }
            else
            {
                print("Falling");
                currentFallingTime -= Time.fixedDeltaTime;
                rb2d.velocity = new Vector2(fallSpeed * -bikeController.currentDir * Time.fixedDeltaTime, rb2d.velocity.y);
            }
        }
    }

    private void FixedUpdate()
    {
        Fall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SunLeeBikeController>() != null &&
            isStunned == false &&
            bikeController.isDestroied == false)
        {
            bikeController.GetComponent<Animator>().SetTrigger("Run");
            bikeController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            bikeController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            bikeController.setBikeMode(true);

            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (isOnBike == true)
        {
            isStunned = true;
            transform.localScale = new Vector2(xSize * -bikeController.currentDir, transform.localScale.y);
            animator.SetTrigger("Fall");
            StartCoroutine(StunForAWhile(stunTime));
        }
    }

    public void TakeExplosionDamage()
    {
        if (health > 0)
        {
            attacking = false;
            int fallDir = Random.Range(0, 2) == 1 ? -1 : 1;
            isStunned = true;
            animator.SetTrigger("Fall");
            transform.localScale = new Vector2(xSize * fallDir * bikeController.currentDir, transform.localScale.y);
            StartCoroutine(StunForAWhile(2));
        }
    }

    IEnumerator StunForAWhile(float duration)
    {
        animator.SetBool("StunnedIdle", isStunned);
        yield return new WaitForSeconds(duration);
        isStunned = false;
        animator.SetBool("StunnedIdle", isStunned);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        if (bikeController.isDestroied)
        {
            isOffBikeMode = true;
        }
    }

    public void SwitchStand()
    {
        //0 = neg, 1 = pos
        colorState = Random.Range(0, 2);
        GameObject changeStyleFXobj = Instantiate(colorState == 0 ? ChangeToNegFX : ChangeToPosFX,
            transform.position, Quaternion.identity);
        Destroy(changeStyleFXobj, 1);
    }

    public override void PlayExplosionSound()
    {
        soundFXHandler.Play("EnemyExplode");
    }

    public void DeathExplosionFX()
    {
        soundFXHandler.Play("ExplodeMech");
    }

    public override void DeathBehaviour()
    {
        //Stop any movement
        rb2d.velocity = Vector2.zero;
        rb2d.bodyType = RigidbodyType2D.Static;

        //Disable all bars
        HealthBarFrameToDisable.SetActive(false);
        blockBar.transform.parent.gameObject.SetActive(false);

        //Disable AI behaviour
        playerToFocus = null;
        //Play destroy animation
        animator.SetTrigger("Death");

        GetComponent<BoxCollider2D>().enabled = false;
        Time.timeScale = 0.002f;
        //Destroy(gameObject);
        soundFXHandler.StopFadeOut("SunleeTheme");

        SunleeLevelHandler.instance.OnBossDead();
    }

    //Bind this function in death animation
    public void startDeathDialogue()
    {
        //Start dialogue
        DialogHandler.instance.startDialogue(outroDialog);
    }

    public override void ParriedBehaviour()
    {
        
    }

    public void StopFall()
    {
        falling = false;
    }

    public void StartFall()
    {
        falling = true;
    }

    public void ApplyDashAttack()
    {
        speed = DashSpeed;
    }

    public void DisableDash()
    {
        speed = WalkSpeed;
    }

    public void ApplyFriction()
    {
        speed = frictionSpeed;
    }

    //Used when playing "switch stand" animation which the character should not move at all
    public void ApplyStaticMove()
    {
        speed = 0;
    }

    public void DisableFriction()
    {
        speed = WalkSpeed;
        canMove = true;
    }

    public void ShootProjectile()
    {
        //Play shoot should
        soundFXHandler.Play("SunleeProjectileLaunch");

        int spawnDir = facingRight ? 1 : -1;

        //spawn 2 - 4 beam with set gap
        for(int i = 0; i < Random.Range(2, 5); i++)
        {
            Instantiate(Beam,
                new Vector3(transform.position.x + spawnDir * 8 * i, transform.position.y + 10, transform.position.z),
                Quaternion.identity);
        }

    }
}

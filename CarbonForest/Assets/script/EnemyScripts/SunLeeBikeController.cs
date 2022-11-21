using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLeeBikeController : EnemyCQC
{
    enum MotionState
    {
        IDLE,
        DASH,
        RANGE,
        SWITCH_STAND
    }

    bool inBikeMode = true;
    
    public static SunLeeBikeController instance;
    SunleeController sunleeController;
    [SerializeField] GameObject smokeObj;
    [SerializeField] GameObject counterAttackFX;

    public GameObject ChangeToNegFX;
    public GameObject ChangeToPosFX;
    
    public GameObject projectile;


    public Transform projectileTransform;

    public Dialog introDialog;
    public bool bikeDestroied;
    public float dashTime;
    float currentDashTime;
    public float dashSpeed;

    public float FXOffset;

    public int parryDamage = 30;

    bool dashing = false;
    bool BikeMode = true;
    bool hasDash = false;

    [HideInInspector]
    public bool turning = false;

    bool waitingForAction;

    public int currentDir = -1;

    int destroyHitCount = 5;//5;

    [SerializeField] GameObject middleStateControler;

    [HideInInspector]
    public bool isDestroied;


    MotionState currentMotionState = MotionState.IDLE;

    public ContinuousExplosionSpawner DestroyExplosionSpawner;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        sunleeController = SunleeController.instance;
        currentDashTime = dashTime;
        StartCoroutine(ActivateBehaviourList());
        animator = GetComponent<Animator>();
       
        Initialize();
        respondRange = 3.5f;
        print(sunleeController);
        blocking = true;
        isDestroied = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inBikeMode)
        {
            //DashToPlayerAtRate();
            OnBikeAttack();
        }
        else
        {
            //EnableBehaviour();
        }
    }

    private void FixedUpdate()
    {
        if (inBikeMode)
        {
            //DashToPlayerAtRate();
            BikeBehaviour();
        }
    }

    public override void AttackPlayer()
    {
        if (canAttack == true)
        {
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    public void OnBikeAttack()
    {
        if (canAttack == true && turning == false)
        {
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        //base.TakeDamage(damage);
        shakeController.CamShake();
        soundFXHandler.Play("SwordCling" + Random.Range(1, 4));

        Instantiate(counterAttackFX,
           new Vector3(
               (facingRight == true ? transform.position.x + FXOffset : transform.position.x - FXOffset),
               transform.position.y,
               transform.position.z),
           Quaternion.identity);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if trigger wall, turn around and keep dashing
        if (collision.gameObject.tag == "ColliderRight" || 
            collision.gameObject.tag == "ColliderLeft")
        {
            if (BikeMode == true)
            {
                currentDashTime = dashTime;
                middleStateControler.SetActive(true);
                GetComponent<SpriteRenderer>().enabled = false;
                turning = true;
                waitingForAction = true;
            }

            rb2d.velocity = Vector2.zero;
            dashing = false;
        }
    }

    //animation event
    public void flipOnTurn()
    {
        currentDir *= -1;
        facingRight = currentDir == 1 ? true : false;
        projectileTransform.transform.localPosition =
            new Vector3(-projectileTransform.transform.localPosition.x,
            projectileTransform.transform.localPosition.y,
            projectileTransform.transform.localPosition.z);
        AdjustStandFXFacing();
    }

    void BikeBehaviour()
    {
        Dash();
    }

    public void DashToPlayerAtRate()
    {
        if (attackRate <= 0)
        {
            dashing = true;
            attackRate = Random.Range(minAttackRate, maxAttackRate);
        }
        else
        {
            attackRate -= Time.deltaTime;
        }
    }

    public override void PlayTakeDamageSound()
    {
        soundFXHandler.Play("BikeDamaged");
    }

    void Dash()
    {
        if (dashing == true)
        {
            if (currentDashTime <= 0)
            {
                dashing = false;
                currentDashTime = dashTime;
                rb2d.velocity = Vector2.zero;
            }
            else
            {
                currentDashTime -= Time.fixedDeltaTime;
                rb2d.velocity = new Vector2(dashSpeed * currentDir * Time.fixedDeltaTime, rb2d.velocity.y);
            }
        }
    }

    public override void ParriedBehaviour()
    {
        if (isDestroied == false)
        {
            if (BikeMode == true)
            {
                //Fall down from bike
                sunleeController.GetComponent<BoxCollider2D>().isTrigger = true;
                animator.SetTrigger("BikeOnlyRun");
                BikeMode = false;
                sunleeController.gameObject.SetActive(true);
                sunleeController.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 0));
                rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
                sunleeController.TakeDamage(parryDamage);
                destroyHitCount -= 1;
                blockBar.gameObject.transform.parent.gameObject.SetActive(false);
                if (destroyHitCount <= 1)
                {
                    isDestroied = true;
                    //Bike destroy FX
                    DestroyExplosionSpawner.PlayExplosion();
                    GetComponent<BoxCollider2D>().enabled = false;
                    sunleeController.blockBar.gameObject.transform.parent.gameObject.SetActive(true);
                    blockBar.gameObject.SetActive(false);
                    StartCoroutine(DisableBikeAfterExplode());
                }

            }
            else
            {
                base.ParriedBehaviour();
            }
        }
    }

    IEnumerator DisableBikeAfterExplode()
    {
        yield return new WaitForSeconds(DestroyExplosionSpawner.GetExplosionDuration());
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void ShowEnemyCurrentStand(bool show)
    {
        if(isDestroied == false)
            base.ShowEnemyCurrentStand(show);
        else
        {
            WhiteStandFX.SetActive(false);
            BlueStandFX.SetActive(false);
        }
    }

    public void ToCombatMode()
    {
        DialogHandler.instance.startDialogue(introDialog);
        DialogHandler.instance.onDialogueEnd = OnDialogFinished;
    }

    public void OnDialogFinished()
    {
        Destroy(smokeObj);
        animator.SetTrigger("ToCombat");
        GetComponent<BossHealthBarComponent>().SetupForCombat();
        if(isDestroied == false)
            soundFXHandler.Play("SunleeTheme");
    }

    public void startAction()
    {
        waitingForAction = true;
        currentMotionState = MotionState.DASH;
    }

    IEnumerator KeepRangeAttack()
    {
        waitingForAction = false;
        animator.SetBool("RangeAttack", true);
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        animator.SetBool("RangeAttack", false);
        waitingForAction = true;
    }

    IEnumerator ActivateBehaviourList()
    {
        MotionState previousState = MotionState.IDLE;
        int attackRound = 0;
        while (true)
        {
            if (waitingForAction && BikeMode == true)
            {
                if (currentMotionState == MotionState.DASH)
                {
                    animator.SetTrigger("Run");
                    previousState = MotionState.DASH;
                }
                else if (currentMotionState == MotionState.RANGE &&
                    Vector2.Distance(playerToFocus.transform.position, transform.position) > respondRange)
                {
                    //if the previous mode is already range, 
                    //no need to play the "transform to gun animation" again
                    if (previousState != MotionState.RANGE)
                    {
                        animator.SetTrigger("ToRange");
                        StartCoroutine(KeepRangeAttack());
                    }
                    else
                    {
                        animator.SetTrigger("Run");
                        currentMotionState = MotionState.DASH;
                    }
                    previousState = MotionState.RANGE;
                }
                else if (currentMotionState == MotionState.SWITCH_STAND)
                {
                    //Play switch stand animation
                    //animator.SetTrigger("SwitchStand");
                    SwitchStand();
                }

                //70 persent chance next attack is dash, to avoid bomb filling the screen
                currentMotionState = (Random.Range(0, 100) > 85) ? MotionState.DASH : MotionState.RANGE;
                attackRound += 1;
                if(attackRound >= Random.Range(2,5))
                {
                    currentMotionState = MotionState.SWITCH_STAND;
                    attackRound = 0;
                }
                //print(currentMotionState);
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    //Trigger this function in "switch stand" animation as animation event
    public void SwitchStand()
    {
        colorState = (colorState == 0 ? 1 : 0);
        GameObject changeStyleFXobj = Instantiate(colorState == 0 ? ChangeToNegFX : ChangeToPosFX,
            transform.position, Quaternion.identity);
        Destroy(changeStyleFXobj, 1);
    }

    public void startDash()
    {
        dashing = true;
        waitingForAction = false;
    }

    public void Launch()
    {
        //launch projectile
        Instantiate(projectile, projectileTransform.position, Quaternion.identity);
        //play launch sound
        soundFXHandler.Play("WeaponFire1");
    }

    public override void PlaySlashSound()
    {
        soundFXHandler.Play("SunleeSowrd");
    }

    public void setBikeMode(bool bikeMode)
    {
        this.BikeMode = bikeMode;
        blockBar.gameObject.transform.parent.gameObject.SetActive(true);
    }
}

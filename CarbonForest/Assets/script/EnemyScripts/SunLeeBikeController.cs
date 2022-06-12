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
    public GameObject projectile;
    public Transform projectileTransform;

    public Dialog introDialog;
    public bool bikeDestroied;
    public float dashTime;
    float currentDashTime;
    public float dashSpeed;

    bool dashing = false;
    bool BikeMode = true;
    bool hasDash = false;

    bool waitingForAction;

    public int currentDir = -1;

    [SerializeField] GameObject middleStateControler;


    MotionState currentMotionState = MotionState.IDLE;

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
        print(sunleeController);
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
        if (canAttack == true)
        {
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange)
            {
                animator.SetTrigger("Attack");
            }
        }
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
        if (BikeMode == true)
        {
            //Fall down from bike
            sunleeController.GetComponent<BoxCollider2D>().isTrigger = true;
            animator.SetTrigger("BikeOnlyRun");
            BikeMode = false;
            sunleeController.gameObject.SetActive(true);
            sunleeController.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 0));
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            base.ParriedBehaviour();
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
                //else if(currentMotionState == MotionState.SWITCH_STAND)
                //{
                //    //Play switch stand animation
                //    animator.SetTrigger("Switch");

                //    //Flip the color state 
                //    colorState = (colorState == 0 ? 1 : 0);


                //}
                //70 persent chance next attack is dash, to avoid bomb filling the screen
                currentMotionState = (Random.Range(0, 100) > 85) ? MotionState.DASH : MotionState.RANGE;
                //print(currentMotionState);
            }
            yield return new WaitForSeconds(.5f);
        }
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
    }
}

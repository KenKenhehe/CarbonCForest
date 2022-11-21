using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechCQCController : EnemyCQC
{
    enum MotionState
    {
        GROUND,
        HOVER,
    }

    enum GroundAttackMode
    {
        Combat,
        Fire
    }
    public GameObject shieldFX;
    public GameObject shieldBreakFX;

    public float WalkSpeed;
    public float hoverHeight = 3;
    float expectedHoverHeight;
    public float takeOffSpeed = 10;

    public float maxHoverTurnAngle = 30f;

    MotionState currentState = MotionState.GROUND;

    GroundAttackMode groundAttackMode;
    public Transform groundFireLaunchPoint;
    public Transform cannonShootTransform;

    public GameObject GroundFireFX;
    public GameObject GroundFireTargetFX;
    public GameObject GroundFireExplosionFX;

    public LayerMask cannonShootLayer;

    float groundAttackSwitchModeTimer = 0;
    float hoverFireTimer = 0;

    bool OnAir = false;
    bool lifting = false;
    bool dropping = false;
    bool shieldBreak = false;

    bool groundHasFire = false;
    bool rotatingToPlayer = false;

    bool hasHoverFireTriggered = false;
    bool hoverCanTurnfacing = true;
    bool cannonLaunching = false;

    bool cannonFireComplete = false;
    public int maxCannonFireNumPerRound = 2;
    int currentCannonFireNum = 0;

    Transform groundTransform;
    Transform airTransform;

    float currentFacingRightAngle;
    float currentFacingLeftAngle;

    public float cannonInitialFireAngle = 25.0f;
    Quaternion angleQuat;
    Vector3 angleVector;
    public GameObject cannonChargeFX;
    public GameObject cannonExplosionFX;
    LineRenderer cannonLine;

    // Start is called before the first frame update
    void Start()
    {
        angleQuat = Quaternion.AngleAxis(cannonInitialFireAngle, Vector3.forward);
        angleVector = angleQuat * Vector3.right;
        print("ANGLE: " + cannonInitialFireAngle);
        groundTransform = transform;
        print("INITIAL: " + groundTransform.position.y);
        expectedHoverHeight = groundTransform.position.y + hoverHeight;
        GetComponent<BossHealthBarComponent>().SetupForCombat();
        groundAttackMode = Random.Range(0, 2) == 1 ? GroundAttackMode.Combat : GroundAttackMode.Fire;
        cannonLine = GetComponent<LineRenderer>();
        cannonLine.enabled = false;
        Initialize();
        StartCoroutine(ActivateBehaviourList());
        // StartCoroutine(CheckGroundModeStateChange());
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
    }

    public override void EnableBehaviour()
    {
        if (currentState == MotionState.GROUND)
            onGroundBehaviour();
        else if (currentState == MotionState.HOVER)
            hoverBehaviour();
    }

    void switchAttackIntention()
    {
        groundAttackMode = Random.Range(0, 4) == 1 ? GroundAttackMode.Fire : GroundAttackMode.Combat;
    }

    void onGroundBehaviour()
    {
        PlayDynamicAnimation();
        ApplyEventWithDuration(ref groundAttackSwitchModeTimer, switchAttackIntention);
        //SwitchGroundAttackIntention();
        if (playerToFocus != null && PlayerGeneralHandler.instance.isDead == false && OnAir == false)
        {
            if (canAttack == true && canMove == true)
                FacePlayer();

            if (groundAttackMode == GroundAttackMode.Combat)
            {
                StopCoroutine(FireFromGround());
                animator.SetBool("GroundShoot", false);
                MoveToPlayerWithRandomRate();
                AttackPlayerAtRate();
            }
            else if (groundAttackMode == GroundAttackMode.Fire)
            {
                rb2d.velocity = Vector2.zero;
                if (groundHasFire == false)
                {
                    animator.SetBool("GroundShoot", true);
                    groundHasFire = true;
                }
            }
        }
    }

    public void SwitchStand()
    {
        //0 = neg, 1 = pos
        colorState = Random.Range(0, 2);
        GameObject changeStyleFXobj = Instantiate(colorState == 0 ? standChangeFXNeg : standChangeFXPos,
            transform.position, Quaternion.identity);
        Destroy(changeStyleFXobj, 1);
    }

    public override void AttackPlayer()
    {
        if (canAttack == true && attacking == false)
        {
            if (Mathf.Abs(transform.position.x - playerToFocus.transform.position.x) < respondRange + 1)
            {
                canMove = false;
                animator.SetTrigger("Attack");
                randHoldTime = Random.Range(.1f, 3f);
                attacking = true;
            }
            else
            {
                canMove = true;
            }
        }
    }

    void hoverBehaviour()
    {
        //Dont change facing when charging and aiming cannon fire
        if (rotatingToPlayer)
            RotateToPlayer();
        else
        {
            if (hoverCanTurnfacing)
            {
                FacePlayer();
                FlipRotation();
            }
            else
            {
                //ResetRotation();
            }
            MoveToPlayerWithRandomRate();
        }
        PlayDynamicHoverAnimation();
        ApplyEventWithDuration(ref hoverFireTimer, HoverFire, Random.Range(3.5f, 5f));
        if (attacking == false)
            checkLifting();
    }

    public void FlipRotation()
    {
        if (playerToFocus != null && playerToFocus.GetComponent<PlayerGeneralHandler>().isDead == false)
        {
            if (facingRight)
            {
                rb2d.rotation = currentFacingRightAngle;
                print("Rotation fliped right");
            }
            else
            {
                rb2d.rotation = currentFacingLeftAngle;
                print("Rotation fliped left");
            }
        }
    }

    void ResetRotation()
    {
        currentFacingLeftAngle = Mathf.Lerp(rb2d.rotation, 0, Time.deltaTime * 2);
        currentFacingRightAngle = Mathf.Lerp(rb2d.rotation, 0, Time.deltaTime * 2);
        rb2d.rotation = Mathf.Lerp(rb2d.rotation, 0, Time.deltaTime * 2);
    }

    void HoverFire()
    {
        if (hasHoverFireTriggered == false && cannonFireComplete == false)
        {
            animator.SetTrigger("HoverShootReady");
            soundFXHandler.Play("CannonShow");
            StartCoroutine(FireFromAir());
            hasHoverFireTriggered = true;
        }
    }

    IEnumerator FireFromAir()
    {
        //Spawn charging FX
        cannonLaunching = true;
        canMove = false;
        int chargeTime = 3;
        soundFXHandler.Play("CannonCharging");
        yield return new WaitForSeconds(chargeTime);
        hoverCanTurnfacing = false;
        animator.SetTrigger("HoverFire");
        hasHoverFireTriggered = false;
        canMove = true;
        cannonLaunching = false;
        currentCannonFireNum += 1;
        if (currentCannonFireNum == maxCannonFireNumPerRound)
            cannonFireComplete = true;
    }

    //Called by animation event when fire
    public void FireCannon()
    {
        shakeController.CamBigShake();
        soundFXHandler.Play("CannonFire");
        float flipedAngle = 180 - (facingRight ? cannonInitialFireAngle : -cannonInitialFireAngle);
        //angleQuat = Quaternion.AngleAxis(facingRight ? flipedAngle : cannonInitialFireAngle, Vector3.forward);
        angleQuat = Quaternion.AngleAxis(flipedAngle, Vector3.forward);
        angleVector = angleQuat * transform.right;

        RaycastHit2D hitinfo = Physics2D.Raycast(cannonShootTransform.position, facingRight ? -angleVector : angleVector, 10000, cannonShootLayer);
        //Debug.DrawRay(transform.position,  -cannonInitialFireAngle);
        //Debug.DrawRay(transform.position, transform.right - cannonInitialFireAngle);
        print("Hit: " + hitinfo.collider);
        print(angleVector);
        if (hitinfo.collider != null)
        {
            cannonLine.enabled = true;
            cannonLine.SetPosition(0, cannonShootTransform.position);
            cannonLine.SetPosition(1, hitinfo.point);
            StartCoroutine(DisableLineInAWhile());
            Instantiate(cannonExplosionFX, hitinfo.point, Quaternion.identity);
        }
        else
        {
            cannonLine.enabled = true;
            cannonLine.SetPosition(0, transform.position);
            cannonLine.SetPosition(1, (-angleVector * 1000));
            StartCoroutine(DisableLineInAWhile());
        }
    }

    IEnumerator DisableLineInAWhile()
    {
        yield return new WaitForSeconds(.1f);
        cannonLine.enabled = false;
    }

    IEnumerator SpawnExplosion()
    {
        yield return new WaitForSeconds(1);
    }

    void RotateToPlayer()
    {
        Vector2 lookDir = playerToFocus.transform.position - transform.position;
        if (facingRight)
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            if (angle > -maxHoverTurnAngle)
            {
                rb2d.rotation = Mathf.Lerp(rb2d.rotation, angle, Time.deltaTime * 2);
                currentFacingRightAngle = angle;
            }
            else
            {
                rb2d.rotation = Mathf.Lerp(rb2d.rotation, -maxHoverTurnAngle, Time.deltaTime * 2);
            }
            //else
            //    rb2d.rotation = Mathf.Lerp(rb2d.rotation, 0, Time.deltaTime * 2);
        }
        else
        {
            float angle = Mathf.Atan2(-lookDir.y, -lookDir.x) * Mathf.Rad2Deg;

            if (angle < maxHoverTurnAngle)
            {
                rb2d.rotation = Mathf.Lerp(rb2d.rotation, angle, Time.deltaTime * 2);
                currentFacingLeftAngle = angle;
            }
            else
            {
                rb2d.rotation = Mathf.Lerp(rb2d.rotation, maxHoverTurnAngle, Time.deltaTime * 2); ;
            }
            //else
            //    rb2d.rotation = Mathf.Lerp(rb2d.rotation, 0, Time.deltaTime * 2);
        }

    }

    void checkLifting()
    {
        if (lifting)
        {
            rb2d.velocity = new Vector2((facingRight ? 1 : -1), takeOffSpeed);
            if (transform.position.y >= expectedHoverHeight)
            {
                lifting = false;
                OnAir = true;
                rb2d.velocity = Vector2.zero;
            }

        }
    }

    public void PlayDynamicHoverAnimation()
    {
        if (rb2d.velocity == Vector2.zero)
        {
            animator.SetBool("IsHoverWalking", false);
        }
        else
        {
            animator.SetBool("IsHoverWalking", true);
        }
    }

    IEnumerator ActivateBehaviourList()
    {
        while (true)
        {
            int actionInterval = 4;
            if (cannonFireComplete)
            {
                if (currentState == MotionState.HOVER)
                {
                    setGroundMode();
                }
                currentState = MotionState.GROUND;
                yield return new WaitForSeconds(1);
                continue;
            }

            yield return new WaitForSeconds(actionInterval);


            if (IsStunned) continue;
            //70% chance on ground, 30% hover
            if (Random.Range(0, 10) < 4 && attacking == false )
            {
                if (canSetToHoverMode())
                {
                    if (currentState == MotionState.GROUND)
                    {
                        setHoverMode();
                    }
                    currentState = MotionState.HOVER;
                }
            }
            else
            {
                if (canSetToGroundMode())
                {
                    if (currentState == MotionState.HOVER)
                    {
                        setGroundMode();
                    }
                    currentState = MotionState.GROUND;
                }
            }

            
        }
    }

    bool canSetToGroundMode()
    {
        return cannonLaunching == false;
    }

    bool canSetToHoverMode()
    {
        return attacking == false;
    }

    void setHoverMode()
    {
        StopCoroutine(FireFromGround());
        //Trigger take off animation
        animator.SetTrigger("TakeOff");

        //disable gravity
        rb2d.gravityScale = 0;

        //move player up certain height
        lifting = true;

        respondRange += 10;

        currentFacingLeftAngle = 0;
        currentFacingRightAngle = 0;
        rb2d.rotation = 0;

    }



    void setGroundMode()
    {
        rb2d.rotation = 0;
        animator.SetTrigger("ToGround");

        respondRange -= 10;
        cannonFireComplete = false;
        currentCannonFireNum = 0;

    }

    IEnumerator FireFromGround()
    {
        int fireCount = Random.Range(3, 5);
        attacking = true;

        //Fire FX
        Instantiate(GroundFireFX, groundFireLaunchPoint.position, Quaternion.identity);
        soundFXHandler.Play("GroundFire");
        yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
        //Spawn target fx at player position
        GameObject targetFXObj = Instantiate(GroundFireTargetFX, playerToFocus.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(1f, 1.7f));
        Instantiate(GroundFireExplosionFX, targetFXObj.transform.position, Quaternion.identity);
        soundFXHandler.Play("EnemyExplode");
        FindObjectOfType<ShakeController>().CamShake();
        Destroy(targetFXObj);


        attacking = false;
        groundHasFire = false;
    }


    //Trigger with animation
    void GroundFire()
    {
        StartCoroutine(FireFromGround());
    }


    //Called by animation event
    void StartDropDown()
    {
        rb2d.gravityScale = 1;

        rb2d.velocity = new Vector2((facingRight ? 1 : -1), -takeOffSpeed * 2);
    }

    public override void TakeDamage(int damage)
    {
        if (shieldBreak)
        {
            base.TakeDamage(damage);
        }
        else
        {
            //Spawn shield
            GameObject sheildFXObj = Instantiate(shieldFX, transform.position,
                facingRight ? transform.rotation : Quaternion.Euler(0, 0, 180));

            //TODO sheild sound

            //Screen shake
            shakeController.CamShake();
        }
    }

    public override void ParriedBehaviour()
    {
        if (shieldBreak == false)
        {
            print("SHIELD STILL HERE");
            //spawn shield break animation
            GameObject sheildBreakFXObj = Instantiate(shieldBreakFX, transform.position,
                facingRight ? transform.rotation : Quaternion.Euler(0, 0, 180));

            //TODO play sound

            //slow down time less that regular parry
            Time.timeScale = 0.4f;

            //screen shake
            shakeController.CamBigShake();
            shieldBreak = true;
        }
        else
            base.ParriedBehaviour();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && OnAir)
        {
            animator.SetTrigger("DropDownTouch");
            OnAir = false;
            rb2d.velocity = Vector2.zero;
        }
    }

    public void FloorImpact()
    {
        shakeController.CamBigShake();
        soundFXHandler.Play("FloorImpact");
    }

    public void ApplyStaticMove()
    {
        speed = 0;
    }

    public void DisableFriction()
    {
        speed = WalkSpeed;
        canMove = true;
    }

    public void setRotatingToPlayer(int i)
    {
        rotatingToPlayer = (i == 0 ? false : true);
        if (rotatingToPlayer)
        {
            GameObject chargeObj = Instantiate(cannonChargeFX, cannonShootTransform.position, Quaternion.identity, transform);
            Destroy(chargeObj, 2.5f);
        }

    }

    public void resetFacing()
    {
        hoverCanTurnfacing = true;
    }
}

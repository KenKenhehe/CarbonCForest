using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelThreeBossController : EnemyCQC
{
    enum AttackMode
    {
        ATTACK_SLASH,
        ATTACK_GROUND,
        ATTACK_DASH
    }

    int[] GroundThresholds = { 290, 240, 210, 180, 120, 70, 30 };
    int thresholdIndex = 0;

    AttackMode currentAttackMode = AttackMode.ATTACK_SLASH;
    int attackCount = 0;
    bool interupt = false;
    bool attacking = false;
    bool parried = false;
    bool hasGrounded = false;
    float accDamage = 0;
    float damageStartTime = 0;
    float attackingCurrentDuration = 0;
    float parriedCurrentDuration = 0;

    public float attackDuration = 2;
    public float parriedDuration = 1;
    public float accDamageTimeLimit = 7;
    public float attackingSpeed = 150;
    public float parriedSpeed = 800;

    public GameObject GroundBreakFX;
    CameraControl camera;
    SoundFXHandler soundFX;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        respondRange = Random.Range(3f, 6f);
        randHoldTime = Random.Range(.1f, 3f);
        camera = FindObjectOfType<CameraControl>();
        print(colorState);
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
        PlayDynamicAnimation();
        print(colorState);
    }

    private void FixedUpdate()
    {
        CalculateDamageDuringTimeLimit();
        if (attacking == true)
        {
            ApplyVelocityWhenAttacking();
        }
        else
        {
            attackingCurrentDuration = 0;
        }

        if (parried == true)
        {
            ApplyParriedVelocity();
        }
        else
        {
            parriedCurrentDuration = 0;
        }
    }

    public override void Initialize()
    {
        hitDuration = new WaitForSeconds(.3f);
        soundFX = SoundFXHandler.instance;
        FindObjectOfType<CameraControl>().camDepth = -3;
        FindObjectOfType<CameraControl>().offsetY = 0.35f;
        base.Initialize();
    }

    void CalculateDamageDuringTimeLimit()
    {
        damageStartTime += Time.fixedDeltaTime;
        if (damageStartTime >= accDamageTimeLimit)
        {
            damageStartTime = 0;
            accDamage = 0;
        }
    }

    public override void AttackPlayer()
    {
        if (canAttack == true & attacking == false)
        {
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange)
            {
                attacking = true;
                AttackBehaviour();
            }
        }
    }

    void AttackBehaviour()
    {
        attackingSpeed = 150;
        attackDuration = 2;
        canAttack = true;
        if (currentAttackMode == AttackMode.ATTACK_SLASH)
        {
            attackRange = 1.5f;
            attackCount += 1;
            animator.SetTrigger("Attack1");
        }
        else if (currentAttackMode == AttackMode.ATTACK_GROUND)
        {

            animator.SetTrigger("Attack2");
            StartCoroutine(DisableAttackForAWhile(2));
        }
        else if (currentAttackMode == AttackMode.ATTACK_DASH)
        {
            animator.SetTrigger("Attack3");
            attackDuration = .5f;
            attackCount = 0;
            canAttack = false;
            attacking = false;
        }
        randHoldTime = Random.Range(.1f, .9f);
    }

    public void ApplyVelocityWhenAttacking()
    {
        attackingCurrentDuration += Time.deltaTime;
        if (attackingCurrentDuration >= attackDuration)
        {
            attackingCurrentDuration = 0;
            attacking = false;
            canAttack = true;
            if (attackCount >= 3)
            {
                currentAttackMode = AttackMode.ATTACK_DASH;
                attackRange = 1.5f;
            }
        }
        else
        {
            rb2d.velocity = new Vector2(
                (facingRight == true ? attackingSpeed : -attackingSpeed) * Time.fixedDeltaTime,
                rb2d.velocity.y
                );
        }
    }

    public void ApplyParriedVelocity()
    {
        parriedCurrentDuration += Time.deltaTime;
        if (parriedCurrentDuration >= parriedDuration)
        {
            parriedCurrentDuration = 0;
            parried = false;
        }
        else
        {
            rb2d.velocity = new Vector2(
                (facingRight == true ? -parriedSpeed : parriedSpeed) * Time.fixedDeltaTime,
                rb2d.velocity.y
                );
        }
    }

    public void ApplyStopSpeed()
    {
        rb2d.velocity = Vector2.zero;
    }

    public override void PlayDynamicAnimation()
    {
        if (rb2d.velocity == Vector2.zero)
        {
            animator.SetBool("IsWalking", false);
        }
        else
        {
            animator.SetBool("IsWalking", true);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(DamagedEffect());
        accDamage += damage;

        if (health < GroundThresholds[thresholdIndex] &&
            health > GroundThresholds[GroundThresholds.Length - 1])
        {
            currentAttackMode = AttackMode.ATTACK_GROUND;
            print("Ground attack!" + thresholdIndex);
            if (thresholdIndex < GroundThresholds.Length - 1)
            {
                thresholdIndex++;
            }
        }

        if (accDamage >= 22)
        {
            //硬直
            print("yingzhi");
        }
    }

    public override void ParriedBehaviour()
    {
        attacking = false;
        parried = true;
    }

    public override void PlaySlashSound()
    {
        if (currentAttackMode == AttackMode.ATTACK_SLASH)
        {
            base.PlaySlashSound();
        }
        else if (currentAttackMode == AttackMode.ATTACK_GROUND)
        {
            soundFX.Play("SwordSwing3");
        }
        else
        {
            base.PlaySlashSound();
        }
    }

    public override void AddiBlockEvent()
    {
        if (currentAttackMode == AttackMode.ATTACK_DASH)
        {
            blockPoint = 0;
            StartCoroutine(changeCamDepth());
            currentAttackMode = AttackMode.ATTACK_SLASH;
        }
    }

    //---Attack combo 2 scripted event---

    public void ImpactOnGround()
    {
        attackingSpeed = 0;
        FindObjectOfType<ShakeController>().CamBigShake();
        StartCoroutine(changeCamDepth());
        currentAttackMode = AttackMode.ATTACK_SLASH;
        //spawn impact FX
        GameObject groundFX =
            Instantiate(GroundBreakFX, transform.position - new Vector3(0, 1f, 0), transform.rotation);
        soundFX.Play("PillarHit");

        //impact ground
        CollapsableController.instance.TakeDamage();
    }

    public void PlaySlashRiseSound()
    {

    }

    public void PlaySlashRotateSound()
    {

    }

    IEnumerator changeCamDepth()
    {
        camera.camDepth = -2;
        camera.player = gameObject;
        yield return new WaitForSeconds(.2f);
        camera.camDepth = -3f;
        camera.player = playerToFocus.gameObject;
        FindObjectOfType<ShakeController>().CamBigShake();
    }

    //---Attack combo 2 event ends --- 

    //--Dash attack event---
    void Charge()
    {

    }

    void Dash()
    {
        attacking = true;
        attackingSpeed = 3500;
        attackRange = 5;
    }
}

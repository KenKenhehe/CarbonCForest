using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : EnemyCQC {
    public GameObject missile;
    public Transform launchPoint;
    public ParticleSystem ChargeFX;
    public GameObject deathFX;

    GameObject chargeFXObj;

    [SerializeField] bool isRangeMode;
    private float intensionSwitchTime = 0;
    float missileCount = 0;

    bool charging = false;

    float dashTime = 1f;
    public float dashSpeed = 1000;
    public bool dashing = false;

    public Slider healthBar;
    SoundFXHandler soundFXHandler;
    
    private void FixedUpdate()
    {
        healthBar.value = health;
        SwitchAttackIntension();
        EnableBehaviour();
        ChangeBlockColorAtRandom();
        if(missileCount >= 5)
        {
            isRangeMode = false;
            charging = true;
            animator.SetBool("RangeAttack", false);
        }

        if(dashing == true)
        {
            if(dashTime <= 0)
            {
                dashing = false;
                dashTime = 2f;
                rb2d.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.fixedDeltaTime;
                rb2d.velocity = new Vector2(
                facingRight == true ? (dashSpeed) * Time.fixedDeltaTime : -(dashSpeed) * Time.fixedDeltaTime,
                rb2d.velocity.y
                ); 
            }
        }
    }

    public void DetactPlayerHit()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 3))
        {
            GameObject hitObj = collider.gameObject;
            if (hitObj.GetComponent<PlayerGeneralHandler>() != null &&
                hitObj.GetComponent<PlayerMovement>().dodging == false)
            {
                hitObj.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(10, 3, this);

                hitObj.transform.position = new Vector2(transform.position.x + (facingRight == true ? 2 : -2),
                    hitObj.transform.position.y + 1);

                FindObjectOfType<ShakeController>().CamBigShake();
            }
            else if(hitObj.GetComponent<PlayerGeneralHandler>() != null &&
                hitObj.GetComponent<PlayerMovement>().dodging == true)
            {
                Time.timeScale = .01f;
            }
        }
    }

    public override void Initialize()
    {
        StartCoroutine(FocusBoss());
        healthBar = FindObjectOfType<Slider>();
        healthBar.GetComponent<Animator>().SetTrigger("Show");
        healthBar.gameObject.SetActive(true);
        healthBar.maxValue = maxHealth;
        base.Initialize();
        stunnedDuration = 2f;
        ChargeFX = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(HoldAndMove());
        soundFXHandler = FindObjectOfType<SoundFXHandler>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(DamagedEffect());
    }

    public override void DeathBehaviour()
    {
        //base.DeathBehaviour();
        animator.SetTrigger("Dead");
        Destroy(gameObject, 1);
        canMove = false;
        GetComponent<Collider2D>().enabled = false;
        rb2d.gravityScale = 0;
    }

    public override void PlayExplosionSound()
    {
        soundFXHandler.Play("EnemyExplode");
    }

    void SwitchAttackIntension()
    {
        if(intensionSwitchTime < 4)
        {
            intensionSwitchTime += Time.deltaTime;
        }
        else
        {
            intensionSwitchTime = 0;
            isRangeMode = Random.Range(0, 2) == 1 ? true : false;
            colorState = Random.Range(0, 2);
            if (blockColorRenderer.color.a > 0)
            {
               ChangeBlockColorAtRandom();
            }
        }
    }
    
    void TriggerEventAfterDead()
    {
        if(GetHealth() <= 1)
        {
            FindObjectOfType<DoorController>().SetToCanOpen("DoorOpenAfterBoss");
        }
    }

    public override void AttackPlayer()
    {
        if (charging == false && dashing == false)
        {
            animator.SetBool("RangeAttack", isRangeMode);
            attackRate = 3f;
            if (isRangeMode == true)
            {
                respondRange = Random.Range(10f, 15f);
            }
            else
            {
                respondRange = Random.Range(5f, 6f);
                CloseAttack();
            }
        }
        else
        {
            animator.SetBool("RangeAttack", false);
            animator.SetTrigger("DashCharging");
            missileCount = 0;
            charging = false;
        }

    }

    void CloseAttack()
    {
        if (canAttack == true)
        {
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange + 1)
            {
                animator.SetTrigger("CQCAttack");
            }
        }
    }

    public override void MoveToPlayer()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.fixedDeltaTime;
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
            animator.SetBool("IsWalking", false);
        }

        if (rb2d.velocity != Vector2.zero)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    public void Dash()
    {
        FacePlayer();
        dashing = true;
        canAttack = false;
    }

    public void FinishDash()
    {
        dashing = false;
        canAttack = true;
        animator.ResetTrigger("DashCharging");
    }

    public void InstantiateMissile()
    {
        soundFXHandler.Play("MissileLaunch");
        missileCount += 1;
        Instantiate(missile,launchPoint.position, Quaternion.identity);
    }

    public void DisableCharging()
    {
        charging = false;
    }

    public void SpawnChargeFX()
    {
        ChargeFX.Play();
    } 

    public void DestoryChargeFX()
    {
        ChargeFX.Stop();
    }

    public void stunnedAfterHitPliiar()
    {
        animator.SetTrigger("Stunned");
        StartCoroutine(DisableAttackForAWhile(7));
        rb2d.velocity = Vector2.zero;
        dashing = false;
        canAttack = false;
    }

    public void DisableAttack()
    {
        canAttack = false;
        rb2d.velocity = Vector2.zero;
    }

    private void OnDestroy()
    {
        healthBar.gameObject.SetActive(false);
        Instantiate(deathFX, transform.position, Quaternion.identity);
    }

    IEnumerator HoldAndMove()
    {
        blockBar.transform.parent.gameObject.SetActive(false);
        blockBar.enabled = false;
        rb2d.gravityScale = 10;
        canMove = false;
        yield return new WaitForSeconds(2);
        canMove = true;
        rb2d.gravityScale = 5;
        blockBar.enabled = true;
        blockBar.transform.parent.gameObject.SetActive(true);
    }

    IEnumerator FocusBoss()
    {
        FindObjectOfType<CameraControl>().player = gameObject;
        //print(FindObjectOfType<CameraControl>().player);
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<CameraControl>().player = playerToFocus.gameObject;
    }

}

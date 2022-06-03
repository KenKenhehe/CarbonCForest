using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("Which weapon player is choosing")]
    int currentWeaponNum = 0;

    [Tooltip("How many weapons do player have")]
    public int currentWeaponCount = 1;

    public float friction = 600;
    public bool attacking;
    public bool Welding = false;

    public float attack1Range = 1f;
    public float attack2Range = 1;
    public float attack3Range = 1.53f;

    public float HeavyAttackRange = 1.6f;

    //public float attackForce = 2500;
    public float attackRangeOffset = 0.69f;
    public float startAttackingTime;
    public LayerMask enemyLayerMask;

    public int attack1Damage;
    public int attack2Damage;
    public int attack3Damage;

    public int HeavyAttackDamage = 6;

    //float startComboTime;
    //float resetTimer = 1f;

    float timesBetweenAttack;
    WaitForSeconds attackAfterSecond = new WaitForSeconds(0.05f);

    Animator animator;

    public Weapon[] weapons;
    public Weapon currentWeapon;

    PlayerMovement playerMovement;
    EnemyShooterController enemyBeingHit;
    ShakeController shakeController;

    SoundFXHandler soundFXHandler;

    public GameObject attackShakeFX;
    public Transform attackShakeTransformRight;
    public Transform attackShakeTransformLeft;

    bool withinAttackConnectionWindow = false;

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Chapter0")
        {
            currentWeaponCount = 2;
        }
        else if(SceneManager.GetActiveScene().name == "Chapter1")
        {
            currentWeaponCount = 0;
            print("Current weapon num is 0");
        }
        else
        {
            currentWeaponCount = Saver.Load().currentWeaponCount;
        }
        animator = GetComponent<Animator>();
        currentWeapon = weapons[currentWeaponNum];
        animator.runtimeAnimatorController = currentWeapon.animatorController;
        playerMovement = GetComponent<PlayerMovement>();
        shakeController = FindObjectOfType<ShakeController>();
        soundFXHandler = SoundFXHandler.instance;
        //WeaponUIHandler.instance.ChangeToCurrentWeaponUI(currentWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInupt();
    }

    void ProcessInupt()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J))
        {
            attacking = true;
            withinAttackConnectionWindow = true;
            Welding = true;
            //StopAllCoroutines();
            currentWeapon.PlayAttackAnimationOnAttackNum(animator);
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("RB"))
        {

            if (currentWeaponNum >= currentWeaponCount)
            {
                currentWeaponNum = 0;
            }
            else
            {
                currentWeaponNum += 1;
            }
            currentWeapon = weapons[currentWeaponNum];
            animator.runtimeAnimatorController = currentWeapon.animatorController;
            WeaponUIHandler.instance.ChangeToCurrentWeaponUI(currentWeapon);
            if (withinAttackConnectionWindow && currentWeaponCount > 0)
            {
                attacking = true;
                animator.SetTrigger("Connect");
            }
        }

        if (attacking == true)
        {
            animator.SetBool("isWalking", false);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            currentWeapon.resetTriggerNames(animator);
        }

        if (attacking && playerMovement != null)
        {
            playerMovement.speed = 50;
        }
    }

    public void AttackAtRightTime(int damage, float range, float shockForce)
    {
        Vector3 attackRangeOrigin = playerMovement.facingRight ?
        transform.position + new Vector3(attackRangeOffset, 0, 0) : transform.position - new Vector3(attackRangeOffset, 0, 0);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackRangeOrigin, range, enemyLayerMask);
        foreach (Collider2D obj in hitObjects)
        {
            Enemy enemy = obj.GetComponent<Enemy>();

            enemy.TakeDamage(damage);

            float ranfomPushbackRange = Random.Range(.5f, 1f);

            enemy.transform.position = new Vector3(
                (playerMovement.facingRight ==
                true ?
                obj.transform.position.x + (shockForce * enemy.unstableness * ranfomPushbackRange)
                :
                obj.transform.position.x - (shockForce * enemy.unstableness * ranfomPushbackRange)),
                obj.transform.position.y,
                obj.transform.position.z);

            if (enemy.blocking == false)
            {
                Instantiate(currentWeapon.slashFX,
                   enemy.transform.position + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.3f, .2f), 0) +
                   (Vector3)enemy.GetComponent<BoxCollider2D>().offset,
                   Quaternion.Euler(0, 0, Random.Range(0, 360)));
            }
        }
    }

    public void AttackAtRightTimeBike(int damage, float range, float shockForce, Vector3 attackRangeOrigin)
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackRangeOrigin, range, enemyLayerMask);
        foreach (Collider2D obj in hitObjects)
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.transform.position = new Vector3(

                obj.transform.position.x - (shockForce * enemy.unstableness),
                obj.transform.position.y,
                obj.transform.position.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(transform.position - new Vector3(attackRangeOffset, 0, 0), attack3Range);
    }

    //AttackN are handled by animation event
    void Attack1()
    {
        if (soundFXHandler != null)
        {
            soundFXHandler.Play("SowrdSwing1");
        }
        AttackAtRightTime(currentWeapon.attack1Damage, currentWeapon.attack1Range, .3f);
    }

    void Attack2()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SowrdSwing2");
        AttackAtRightTime(currentWeapon.attack2Damage, currentWeapon.attack2Range, .4f);
    }

    void Attack3()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SowrdSwing1");
        AttackAtRightTime(currentWeapon.attack3Damage, currentWeapon.attack3Range, 1f);
    }

    void SpearAttack1()
    {
        if (soundFXHandler != null)
        {
            soundFXHandler.Play("SpearWhoosh" + Random.Range(1, 4));
        }
        AttackAtRightTime(currentWeapon.attack1Damage, currentWeapon.attack1Range, .3f);
    }
    void SpearAttack2()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SpearWhoosh" + Random.Range(1, 4));
        AttackAtRightTime(currentWeapon.attack2Damage, currentWeapon.attack2Range, .4f);
    }

    void SpearAttack3()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SpearWhoosh" + Random.Range(1, 4));
        AttackAtRightTime(currentWeapon.attack2Damage, currentWeapon.attack3Range, 1f);
    }

    void SpearAttack4()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SpearWhoosh" + Random.Range(1, 4));
        AttackAtRightTime(Spear.attack4Damage, Spear.attack4Range, 1f);
    }

    void SpearAttack5()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SpearWhoosh" + Random.Range(1, 4));
        AttackAtRightTime(Spear.attack5Damage, Spear.attack5Range, 1f);
    }

    void SpearHeavyAttack()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SpearHeavyWhoosh" + Random.Range(1, 5));
        AttackAtRightTime(Spear.heavyAttackDamage, Spear.heavyAttackRange, .5f);
    }

    void HeavyAttackSwingSFX()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SpearWhoosh2");
    }

    void SpearPlasmaSFX()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SpearHeavyPlasma");
    }

    void HeavyAttackCombineSFX()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SpearHeavyCombine");
    }

    void BikeAttack1()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SowrdSwing1");
        AttackAtRightTimeBike(attack1Damage, attack1Range, .4f,
            transform.position - new Vector3(attackRangeOffset, 0, 0));
    }

    void BikeAttack2()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SowrdSwing2");
        AttackAtRightTimeBike(attack2Damage, attack2Range, .4f,
            transform.position - new Vector3(attackRangeOffset, 0, 0));
    }

    void BikeAttack3()
    {
        if (soundFXHandler != null)
            soundFXHandler.Play("SowrdSwing2");
        AttackAtRightTimeBike(attack3Damage, attack3Range, 1f,
            transform.position);
    }

    //Handled by animation event, triggers when player animation is not attack animation
    void DisableAttack()
    {
        StopAllCoroutines();
        StartCoroutine(waitForAttack());
        StartCoroutine(waitForattackConnectionWindow());
    }

    //Animation event function, triggers when attacking
    void EnableFriction()
    {
        //if (playerMovement.dodging == false)
        //{
        //    playerMovement.speed = 50;
        //}
    }

    //Handled by animation event, triggers when player animation is not attack animation
    void DisableFriction()
    {
        if(playerMovement != null)
            playerMovement.speed = playerMovement.walkSpeed;
    }

    void EnableHeavyMovemet()
    {
        if (playerMovement.dodging == false)
            playerMovement.speed = 0;
    }

    void DisableHeavyMovement()
    {
        playerMovement.speed = playerMovement.walkSpeed;
    }

    IEnumerator waitForAttack()
    {
        yield return new WaitForSeconds(.15f);
        attacking = false;
        
    }

    IEnumerator waitForattackConnectionWindow()
    {
        yield return new WaitForSeconds(.5f);
        withinAttackConnectionWindow = false;
    }

    void AttackShakeFX()
    {
        shakeController.CamShake();
        //Spawn FX here
        if (currentWeapon.attackShakeFX != null)
        {
            Instantiate(currentWeapon.attackShakeFX,
                (playerMovement.facingRight == true ?
                attackShakeTransformRight :
                attackShakeTransformLeft).position + new Vector3(0, 0.6f, 0),
                Quaternion.identity);
        }
        //Spawn Sound here
        soundFXHandler.Play("WeaponFloorImpact");
    }
}

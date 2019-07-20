using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour {
    public int currentWeaponNum = 1;

    public float friction = 600;
    public bool attacking;

    public float attack1Range = 0.4f;
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

    float startComboTime;
    float resetTimer = 1f;

    float timesBetweenAttack;
    WaitForSeconds attackAfterSecond = new WaitForSeconds(0.05f);

    Animator animator;

    public enum Weapon{Sword, Spear, Bike};
    public Weapon currentWeapon;

    public RuntimeAnimatorController weapon1Controller;
    public RuntimeAnimatorController weapon2Controller;
    public RuntimeAnimatorController weapon3Controller;
    Rigidbody2D rb2d;
    PlayerMovement playerMovement;
    BoxCollider2D bx2d;
    EnemyShooterController enemyBeingHit;
    ShakeController shakeController;
    SpriteRenderer renderer;
    CameraControl cameraControl;
    public string[] attackTriggerNames = new string[] { "Attack", "Attack2", "Attack3"};
    public string[] spearAttackTriggerNames = new string[] { "Attack", "Attack2", "Attack3", "Attack4", "Attack5" };
    public string[] HeavyAttackTriggerNames = new string[] { "HeavyAttack1", "HeavyAttack2", "HeavyAttack3" };

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        if(currentWeapon == Weapon.Sword)
        {
            animator.runtimeAnimatorController = weapon1Controller;
        }
        else if (currentWeapon == Weapon.Spear)
        {
            animator.runtimeAnimatorController = weapon2Controller;
            attack1Range = 1.5f;
            attack2Range = 1.6f;
            attack3Range = 4.25f;
            attack1Damage = 1;
            attack2Damage = 1;
            attack3Damage = 2;
        }
        rb2d = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        bx2d = GetComponent<BoxCollider2D>();
        shakeController = FindObjectOfType<ShakeController>();
        renderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        ProcessInupt();
	}

    void ProcessInupt()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J))
        {
            attacking = true;
            if (currentWeapon == Weapon.Spear)
            {
                PlaySpearAttackAnimationOnAttackNum();
                
            }
            else if(currentWeapon == Weapon.Sword)
            {
                attack1Range = 0.4f;
                attack2Range = 1;
                attack3Range = 1.53f;
                PlayAttackAnimationOnAttackNum();
               
            }
        }
        if(currentWeapon == Weapon.Spear && Input.GetKeyDown(KeyCode.E) )
        {
            currentWeapon = Weapon.Sword;
            animator.runtimeAnimatorController = weapon1Controller;
            attack1Range = 0.4f;
            attack2Range = 1;
            attack3Range = 1.53f;
            attack1Damage = 2;
            attack2Damage = 2;
            attack3Damage = 4;
        }
        else if(currentWeapon == Weapon.Sword && Input.GetKeyDown(KeyCode.E))
        {
            currentWeapon = Weapon.Spear;
            animator.runtimeAnimatorController = weapon2Controller;
            attack1Range = 1.5f;
            attack2Range = 1.6f;
            attack3Range = 4.25f;
            attack1Damage = 1;
            attack2Damage = 1;
            attack3Damage = 2;
        }

        if (attacking == true)
        {
            animator.SetBool("isWalking", false);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            foreach(string attackTriggerName in attackTriggerNames)
            {
                if(attackTriggerName != "Attack")
                {
                    animator.ResetTrigger(attackTriggerName);
                }
            }

            foreach (string HeavyAttackName in HeavyAttackTriggerNames)
            {
                if (HeavyAttackName != "HeavyAttack1")
                {
                    animator.ResetTrigger(HeavyAttackName);
                }
            }
            foreach (string spearAttackTriggerName in spearAttackTriggerNames)
            {
                if (spearAttackTriggerName != "Attack")
                {
                    animator.ResetTrigger(spearAttackTriggerName);
                }
            }

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
            enemy.transform.position = new Vector3(
                (playerMovement.facingRight == 
                true ? 
                obj.transform.position.x + (shockForce * enemy.unstableness) 
                : 
                obj.transform.position.x - (shockForce * enemy.unstableness)),
                obj.transform.position.y,
                obj.transform.position.z);
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

    //only plays the animation, damage is handled elsewhere...
    void PlayAttackAnimationOnAttackNum()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            animator.SetTrigger(attackTriggerNames[0]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("KinghtAttack1"))
        {
            animator.SetTrigger(attackTriggerNames[1]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            animator.SetTrigger(attackTriggerNames[2]);
        }
    }

    void PlaySpearAttackAnimationOnAttackNum()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            animator.SetTrigger(spearAttackTriggerNames[0]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            animator.SetTrigger(spearAttackTriggerNames[1]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            animator.SetTrigger(spearAttackTriggerNames[2]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            animator.SetTrigger(spearAttackTriggerNames[3]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack4"))
        {
            animator.SetTrigger(spearAttackTriggerNames[4]);
        }
    }

    public void PlayHeavyAttackAni()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            animator.SetTrigger(HeavyAttackTriggerNames[0]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("HeavyAttack1"))
        {
            animator.SetTrigger(HeavyAttackTriggerNames[1]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("HeavyAttack2"))
        {
            animator.SetTrigger(HeavyAttackTriggerNames[2]);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position - new Vector3(attackRangeOffset, 0, 0), attack3Range);
    }

    //AttackN are handled by animation event
    void Attack1()
    {
        if (FindObjectOfType<SoundFXHandler>() != null)
        {
            FindObjectOfType<SoundFXHandler>().Play("SowrdSwing1");
        }
        AttackAtRightTime(attack1Damage, attack1Range, .3f);
    }

    void Attack2()
    {
        if (FindObjectOfType<SoundFXHandler>() != null)
            FindObjectOfType<SoundFXHandler>().Play("SowrdSwing2");
        AttackAtRightTime(attack2Damage, attack2Range, .4f);
    }

    void Attack3()
    {
        if (FindObjectOfType<SoundFXHandler>() != null)
            FindObjectOfType<SoundFXHandler>().Play("SowrdSwing2");
        AttackAtRightTime(attack3Damage, attack3Range, 1f);
    }

    void BikeAttack1()
    {
        AttackAtRightTimeBike(attack1Damage, attack1Range, .4f, 
            transform.position - new Vector3(attackRangeOffset, 0, 0));
    }

    void BikeAttack2()
    {
        AttackAtRightTimeBike(attack2Damage, attack2Range, .4f,
            transform.position - new Vector3(attackRangeOffset, 0, 0));
    }

    void BikeAttack3()
    {
        AttackAtRightTimeBike(attack3Damage, attack3Range, 1f,
            transform.position);
    }

    //Handled by animation event, triggers when player animation is not attack animation
    void DisableAttack()
    {
        StartCoroutine(waitForAttack());
    }

    //Animation event function, triggers when attacking
    void EnableFriction()
    {
        if (playerMovement.dodging == false)
        {
            playerMovement.speed = 50;
        }        
    }

    //Handled by animation event, triggers when player animation is not attack animation
    void DisableFriction()
    {
        playerMovement.speed = playerMovement.walkSpeed;
    }

    IEnumerator waitForAttack()
    {
        yield return new WaitForSeconds(.15f);
        attacking = false;
    }
}

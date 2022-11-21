using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteCQCHandler : EnemyCQC
{
    // Start is called before the first frame update
    bool[] attacksFinished;
    int currentAttackIndex = 0;

    public GameObject counterAttackFX;
    public float FXOffset;

    public float DashSpeed = 1500;
    public float WalkSpeed = 100;

    int currentCounterAttack = 1;
    //This is use to control movement during attack, triggered by animation event
    bool canAttackMove = false;
    void Start()
    {
        attacksFinished = new bool[3];
        for(int i = 0; i < attacksFinished.Length; i++)
        {
            attacksFinished[i] = false;
        }
        speed = WalkSpeed;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
    }

    public override void EnableBehaviour()
    {
        PlayDynamicAnimation();
        if (playerToFocus != null && PlayerGeneralHandler.instance.isDead == false && IsStunned == false)
        {
            if (canAttack == true && canMove == true)
                FacePlayer();

            if (canAttackMove)
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
      
    }

    public void FollowUpAttack()
    {
        MoveToPlayer();
        if (attacking)
        {
            print("SET FOLLOWUP");
            animator.SetTrigger("FollowUpAttack");
            attacking = false;
            animator.SetTrigger("Attack1");
        }
    }

    public override void TakeDamage(int damage)
    {
        if (blockPoint > 0)
        {
            BlockPlayer();
            health -= 1;
            UpdateHealthUI();
        }
        else
        {
            base.TakeDamage(damage);
        }
    }

    public override void AttackPlayer()
    {
        print("Sunlee attack");
        if (canAttack == true)
        {
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange + 1 &&
                attacking == false)
            {
                canMove = false;
                animator.SetTrigger("Attack1");
                attacking = true;
            }
        }
    }


    //Only set animation trigger when previous animation is finished, 
    //and randomly decide whether a stand switch should happen
    public void attackFinished(int i)
    {
        if (i == 1)
        {
            attacksFinished[0] = true;
            if (Random.Range(0, 2) == 0)
            {
                animator.SetTrigger("SwitchStand");
            }
            animator.SetTrigger("Attack2");
            
        }
        else if (i == 2)
        {
            attacksFinished[1] = true;
            if (Random.Range(0, 2) == 0)
            {
                animator.SetTrigger("SwitchStand");
            }
            animator.SetTrigger("Attack3");
           
        }
        else if (i == 3)
        {
            attacksFinished[2] = true;
            //animator.SetTrigger("Attack" + i.ToString());
            ResetAnimationState();
        }
        FacePlayer();
    }

    public void SwitchStand()
    {
        //0 = neg, 1 = pos
        colorState = Random.Range(0, 2);
        GameObject changeStyleFXobj = Instantiate(colorState == 0 ? standChangeFXNeg : standChangeFXPos,
            transform.position, Quaternion.identity);
        Destroy(changeStyleFXobj, 1);
    }

    void ResetAnimationState()
    {
        attacking = false;
        for (int j = 0; j < attacksFinished.Length; j++)
        {
            attacksFinished[j] = false;
        }
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("SwitchStand");
        canMove = true;
    }

    void BlockPlayer()
    {
        if (currentAttackIndex > 2)
            currentAttackIndex = 1;
        ShakeController.instance.CamShake();
        animator.SetTrigger("Counter" + currentAttackIndex.ToString());
        soundFXHandler.Play("SwordCling" + Random.Range(1, 5));
        StartCoroutine(BlockForAWhileAndResumeAttack());
        Instantiate(counterAttackFX,
           new Vector3(
               (facingRight == true ? transform.position.x + FXOffset : transform.position.x - FXOffset),
               transform.position.y,
               transform.position.z),
           Quaternion.identity);

        transform.position = new Vector3(
                (facingRight ==
                true ?
                transform.position.x + (shockForce * unstableness) * 0.5f
                :
                transform.position.x - (shockForce * unstableness) * 0.5f),
                transform.position.y,
                transform.position.z);
        currentAttackIndex += 1;
        attacking = false;
    }
    IEnumerator BlockForAWhileAndResumeAttack()
    {
        canAttack = false;
        canMove = false;
        blocking = true;
        yield return new WaitForSeconds(1.5f);
        canMove = true;
        canAttack = true;
        blocking = false;
    }

    //Apply these as animation events

    public void ApplyDashAttack()
    {
        speed = DashSpeed;
    }

    public void DisableDash()
    {
        speed = WalkSpeed;
    }

    public void StopMovement()
    {
        speed = 0;
    }

    public void EnableMovement()
    {
        speed = WalkSpeed;
    }

    public void SetAttackMove(int state)
    {
        canAttackMove = state == 1 ? true : false;
        this.speed = 300;
        if (canAttackMove)
            FacePlayer();
    }
}

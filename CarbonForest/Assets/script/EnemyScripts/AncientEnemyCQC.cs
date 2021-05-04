using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientEnemyCQC : EnemyCQC
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        soundFXHandler.Play("Cloth" + Random.Range(1, 3));
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
        PlayDynamicAnimation();
    }

    public override void PlayDynamicAnimation()
    {
        if (facingRight == false)
        {
            if (rb2d.velocity.x < 0)
            {
                animator.SetBool("WalkBackward", false);
                animator.SetBool("WalkForward", true);
            }
            else if (rb2d.velocity.x > 0)
            {
                animator.SetBool("WalkForward", false);
                animator.SetBool("WalkBackward", true);
            }
            else
            {
                animator.SetBool("WalkForward", false);
                animator.SetBool("WalkBackward", false);
            }
        }
        else
        {
            if (rb2d.velocity.x < 0)
            {
                animator.SetBool("WalkBackward", true);
                animator.SetBool("WalkForward", false);
            }
            else if (rb2d.velocity.x > 0)
            {
                animator.SetBool("WalkForward", true);
                animator.SetBool("WalkBackward", false);
            }
            else
            {
                animator.SetBool("WalkForward", false);
                animator.SetBool("WalkBackward", false);
            }
        }
    }

    public override void AttackPlayer()
    {
        if (canAttack == true)
        {
            if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange + 1)
            {
                canMove = false;
                animator.SetTrigger("Attack1");
                animator.SetTrigger("Attack2");
                animator.SetTrigger("Attack3");
                randHoldTime = Random.Range(.1f, 3f);
            }
            else
            {
                canMove = true;
            }
        }
        else
        {
            animator.ResetTrigger("Attack1");
        }
    }

    public override void MoveToPlayer()
    {
        base.MoveToPlayer();
    }

}

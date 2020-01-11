using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BikeAttacker : Weapon
{
    public string[] attackTriggerNames = new string[] { "Attack", "Attack2", "Attack3" };
    public string[] HeavyAttackTriggerNames = new string[] { "HeavyAttack1", "HeavyAttack2", "HeavyAttack3" };
    public override void PlayAttackAnimationOnAttackNum(Animator animator)
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

    public override void PlayHeavyAttackAnimationOnAttackNum(Animator animator)
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

    public override void resetTriggerNames(Animator animator)
    {
        foreach (string attackTriggerName in attackTriggerNames)
        {
            if (attackTriggerName != "Attack")
            {
                animator.ResetTrigger(attackTriggerName);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SwordBreaker : Weapon
{
    public string[] breakerAttackTriggerNames = new string[] { "Attack", "Attack2", "Attack3", "Attack4"};

    public override void PlayAttackAnimationOnAttackNum(Animator animator)
    {
        base.PlayAttackAnimationOnAttackNum(animator);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
           || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            animator.SetTrigger(breakerAttackTriggerNames[0]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            animator.SetTrigger(breakerAttackTriggerNames[1]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            animator.SetTrigger(breakerAttackTriggerNames[2]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            animator.SetTrigger(breakerAttackTriggerNames[3]);
        }
    }
}

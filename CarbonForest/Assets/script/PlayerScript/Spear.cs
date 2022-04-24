using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Spear : Weapon
{
    public static float attack4Range = 4.25f;
    public static float attack5Range = 4.25f;

    public static int attack4Damage = 2;
    public static int attack5Damage = 2;

    public static float heavyAttackRange = 4.25f;
    public static int heavyAttackDamage = 2;

    public string[] spearAttackTriggerNames = new string[] { "Attack", "Attack2", "Attack3", "Attack4", "Attack5" };
    public string[] HeavyAttackTriggerNames = new string[] { "HeavyAttack1", "HeavyAttack2", "HeavyAttack3", "HeavyAttack4", "HeavyAttack5"};

    public override void PlayAttackAnimationOnAttackNum(Animator animator)
    {
        base.PlayAttackAnimationOnAttackNum(animator);
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
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("HeavyAttack3"))
        {
            animator.SetTrigger(HeavyAttackTriggerNames[3]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("HeavyAttack4"))
        {
            animator.SetTrigger(HeavyAttackTriggerNames[4]);
        }
    }
    public override void resetTriggerNames(Animator animator)
    {
        foreach (string attackTriggerName in spearAttackTriggerNames)
        {
            if (attackTriggerName != "Attack")
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

    }
}

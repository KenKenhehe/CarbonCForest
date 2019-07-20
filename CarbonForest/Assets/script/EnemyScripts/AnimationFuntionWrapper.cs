using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFuntionWrapper : MonoBehaviour {

    public void EnemyAttack()
    {
        GetComponentInParent<EnemyCQC>().ApplyDamage();
        GetComponentInParent<EnemyCQC>().stunnedDuration = 2;
    }

    public void RangeAttack()
    {
        GetComponentInParent<BossController>().InstantiateMissile();
    }

    public void InstantiateChargeFX()
    {
        GetComponentInParent<BossController>().SpawnChargeFX();
    }

    public void DestoryChargeFX()
    {
        GetComponentInParent<BossController>().DestoryChargeFX();
    }

    public void EnableDash()
    {
        GetComponentInParent<BossController>().Dash();
    }

    public void DisableDash()
    {
        GetComponentInParent<BossController>().FinishDash();
    }

    public void DisableEnemyAttack()
    {
        GetComponentInParent<BossController>().DisableAttack();
    }

    public void DashHit()
    {
        GetComponentInParent<BossController>().DetactPlayerHit();
    }

    public void Shake()
    {
        FindObjectOfType<ShakeController>().CamBigShake();
    }
    public void Explode()
    {
        FindObjectOfType<SoundFXHandler>().Play("EnemyExplode");
    }
}

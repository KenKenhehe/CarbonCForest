using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : EnemyShooterController
{
    public bool inTutorial;
    int arrowCount;
    GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        soundFXHandler.Play("Cloth" + Random.Range(1, 3));
    }

    // Update is called once per frame
    void Update()
    {
        if(playerToFocus != null && isDead == false)
        {
            if (canMove == true)
            {
                FacePlayer();
                MoveToPlayer();
                PlayDynamicAnimation();
            }
            AttackPlayer();
           
        }
    }

    public override void AttackPlayer()
    {
        fireTime += Time.deltaTime;
        if (fireTime >= fireRate && takingDamage == false)
        {
            animator.SetTrigger("Fire");
            fireTime = 0;
        }
    }
    
    public void FireArrow()
    {
        arrow = Instantiate(bulletPref, transform.position + new Vector3(0, -.7f, 0), Quaternion.identity);
        arrow.GetComponent<BulletController>().enemy = this;
        SoundFXHandler.instance.Play("ArrowShot");
    }

    public void walkAgain()
    {
        canMove = true;
    }

    public void DisableWalk()
    {
        canMove = false;
    }

    public void DrawArrow()
    {
        SoundFXHandler.instance.Play("ArrowLoad");
    }

    public override void PlayTakeDamageSound()
    {
        soundFXHandler.Play("DamageFlesh" + Random.Range(1, 3));
    }

    public override void DeathBehaviour()
    {
        DeathWithAnimation();
        enabled = false;
    }
}

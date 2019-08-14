using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : EnemyShooterController
{
    bool canWalk = true;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerToFocus != null)
        {
            MoveTowardsPlayer();
            if (canWalk == true)
            {
                FacePlayer();
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
            canWalk = false;
            fireTime = 0;
        }
    }
    
    public void FireArrow()
    {
        Instantiate(bulletPref, transform.position, Quaternion.identity);
        
    }

    public void walkAgain()
    {
        canWalk = true;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : EnemyShooterController
{
    public bool inTutorial;
    int arrowCount;
    bool canWalk = true;
    GameObject arrow;
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
        arrow = Instantiate(bulletPref, transform.position + new Vector3(0, -.7f, 0), Quaternion.identity);
    }

    public void walkAgain()
    {
        canWalk = true;
    }
}

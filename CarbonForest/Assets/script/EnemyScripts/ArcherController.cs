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
            if (canWalk == true)
            {
                FacePlayer();
                MoveTowardsPlayer();
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
    }

    public void walkAgain()
    {
        canWalk = true;
    }

    public void DisableWalk()
    {
        canWalk = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherControllerTutorial : EnemyShooterController
{
    public bool inTutorial;
    int arrowCount;
    bool canWalk = true;
    GameObject arrow;
    TutorialManagerZero tutorial;
    public bool isSlowMotion;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        tutorial = FindObjectOfType<TutorialManagerZero>();
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
            if (arrow != null && tutorial.hasBlock == false)
            {
                if (arrowCount <= 1 && Vector2.Distance(arrow.transform.position, playerToFocus.transform.position) < 7)
                {
                    isSlowMotion = true;
                    TutorialManagerZero.InTutorial = true;
                }
            }

            if(isSlowMotion == true)
            {
                if(Time.timeScale >= 0.05)
                    Time.timeScale -= Time.deltaTime * 4f;
                if(tutorial.hasBlock == false)
                {
                    isSlowMotion = false;
                }
            }
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
        arrowCount += 1;
    }

    public void walkAgain()
    {
        canWalk = true;
    }
}

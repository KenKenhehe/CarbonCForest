using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherControllerTutorial : EnemyShooterController
{
    public float contactDistance = 7;
    public Vector3 arrowOffset; 

    int arrowCount;
    public bool canWalk = true;
   
    GameObject arrow;
    TutorialManagerZero tutorial;
    // Start is called before the first frame update
    void Start()
    {
        arrowOffset = new Vector3(0, -.7f, 0);
        Initialize();
        tutorial = TutorialManagerZero.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerToFocus != null)
        {
            if (canWalk == true)
            {
                MoveToPlayer();
                FacePlayer();
                PlayDynamicAnimation();
            }
            AttackPlayer();
            if (arrow != null && tutorial.hasBlock == false)
            {
                if (arrowCount <= 1 && Vector2.Distance(arrow.transform.position, playerToFocus.transform.position) < contactDistance)
                {
                    tutorial.isSlowMotion = true;
                    TutorialManagerZero.InTutorial = true;
                }
            }

            if (tutorial.isSlowMotion == true)
            {
                if (Time.timeScale >= 0.05)
                    Time.timeScale -= Time.deltaTime * 4f;
                if (tutorial.hasBlock == false)
                {
                    tutorial.isSlowMotion = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        
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

    public void DisableWalk()
    {
        canWalk = false;
    }

    public void FireArrow()
    {
        arrow = Instantiate(bulletPref, transform.position + arrowOffset, Quaternion.identity);
        arrow.GetComponent<BulletController>().enemy = this;
        arrowCount += 1;
        SoundFXHandler.instance.Play("ArrowShot");
    }

    public void walkAgain()
    {
        canWalk = true;
    }

    public void DrawArrow()
    {
        SoundFXHandler.instance.Play("ArrowLoad");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : EnemyShooterController {
    public Transform firePoint;
	// Use this for initialization
	void Start () {
        Initialize();
        StartCoroutine(MoveAfterShowAnimation());
        soundFXHandler.Play("TurrentShow" + Random.Range(1, 4));
        fireRate = Random.Range(.8f, 1.6f);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (canMove)
        {
            MoveToPlayer();
            AttackPlayer();
        }
	}

    public override void AttackPlayer()
    {
        fireTime += Time.deltaTime;
        if (fireTime >= fireRate && takingDamage == false)
        {
            if (facingRight == true)
            {
                animator.SetTrigger("AttackRight");
            }
            else
            {
                animator.SetTrigger("Attack");
            }
            fireTime = 0;
        }
    }

    void MoveToPlayer()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.deltaTime;
        }

        if (playerToFocus.transform.position.x - respondRange > transform.position.x)
        {
            rb2d.velocity = new Vector2(speed * Time.deltaTime, rb2d.velocity.y);
        }
        else if (playerToFocus.transform.position.x + respondRange < transform.position.x)
        {
            rb2d.velocity = new Vector2(-(speed * Time.deltaTime), rb2d.velocity.y);
        }

        if(transform.position.x > playerToFocus.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        if(rb2d.velocity.x != 0)
        {
            if (facingRight == true)
            {
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsWalkingLeft", false);
            }
            else
            {
                animator.SetBool("IsWalkingLeft", true);
                animator.SetBool("IsWalking", false);
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsWalkingLeft", false);
        }
    }

    void Fire() {
        FindObjectOfType<SoundFXHandler>().Play("LaunchBullet");
        Instantiate(bulletPref, firePoint.position, Quaternion.identity);
    }

    public override void PlayExplosionSound()
    {
        FindObjectOfType<SoundFXHandler>().Play("EnemyExplode");
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(DamagedEffect());
    }

    IEnumerator MoveAfterShowAnimation()
    {
        canMove = false;
        yield return new WaitForSeconds(2f);
        canMove = true;
    }
}

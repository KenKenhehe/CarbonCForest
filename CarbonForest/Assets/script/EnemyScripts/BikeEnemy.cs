using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeEnemy : Enemy {
    GameObject target;
    float randomRange;
    float randomAttackPeriod;
    Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        health = Random.Range(10, 15);
        target = FindObjectOfType<MotoController>().gameObject;
        randomRange = Random.Range(2f, 4f);
        randomAttackPeriod = Random.Range(1.5f, 3f);
        shakeController = ShakeController.instance;
    }
	
	// Update is called once per frame
	void Update () {
        MoveToPlayer();
        if(randomAttackPeriod > 0)
        {
            randomAttackPeriod -= Time.deltaTime;
        }
        else
        {
            AttackPlayer();
            randomAttackPeriod = Random.Range(1.5f, 3f);
            randomRange = Random.Range(2f, 4f);
        }
	}

    public override void AttackPlayer()
    {
        animator.SetTrigger("Attack");
    }

    public override void TakeDamage(int damage)
    {
        Time.timeScale = Random.Range(.3f, .5f);
        base.TakeDamage(damage);
        health -= damage;
        FindObjectOfType<ShakeController>().CamShake();
        SoundFXHandler.instance.Play("BikeDamaged");
        Instantiate(bloodFX, transform.position + Vector3.down, Quaternion.identity);
        if (health <= 1)
        {
            FindObjectOfType<ShakeController>().CamBigShake();
            Destroy(gameObject);
            Instantiate(explosionFXs[Random.Range(0, explosionFXs.Length)], transform.position + Vector3.down, Quaternion.identity);
            Instantiate(destoryFX, transform.position + Vector3.down, Quaternion.identity);
            PlayExplosionSound();
        }
        StartCoroutine(DamagedEffect());
    }

    public void MoveToPlayer()
    {
        transform.position = new Vector2(
            Mathf.Lerp(transform.position.x, target.transform.position.x - randomRange, speed * Time.deltaTime),
            transform.position.y);
    }

    public override void PlayExplosionSound()
    {
        SoundFXHandler.instance.Play("MissileExplode");
    }

    public void ApplyDamage()
    {
        print("Damaged player");
    }

    //public override void PlayTakeDamageSound()
    //{
    //    SoundFXHandler.instance.Play("BikeDamaged")
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public float unstableness = 1;
    public int minHealth = 10;
    public int maxHealth = 20;

    public int blockPoint = 2;
    public int maxBlockPoint = 3;

    protected PlayerAttack playerToFocus;
    protected float health;
    protected float startHealth;
    protected float SightRange;
    protected bool PlayerSeen = false;

    public float maxSightRange;
    public float minSightRange;

    

    public Image healthBar;

    public GameObject bloodFX;
    public GameObject destoryFX;
    public GameObject[] explosionFXs;
    public float speed;
    public float maxX;
    public float minX;
    public float fallMultiplier = 2;

    public SpriteRenderer blockColorRenderer;

    protected WaitForSeconds hitDuration;

    public float xSize = 2;

    public bool facingRight;
    public bool takingDamage = false;
    public bool canMove = true;

    public Color damagedColor;
    public Color originColor;

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (healthBar != null)
        {
            healthBar.fillAmount = health / startHealth;
        }
        StartCoroutine(DamagedState());
    }

    public virtual void AttackPlayer()
    {

    }

    public void FacePlayer()
    {
        if (playerToFocus.transform.position.x > transform.position.x)
        {
            facingRight = true;
            transform.localScale = new Vector2(-xSize, transform.localScale.y);
        }
        else
        {
            facingRight = false;
            transform.localScale = new Vector2(xSize, transform.localScale.y);
        }
    }

    public IEnumerator DamagedEffect()
    {
        GetComponentInChildren<SpriteRenderer>().color = damagedColor;

        yield return hitDuration;

        GetComponentInChildren<SpriteRenderer>().color = originColor;
    }

    IEnumerator DamagedState()
    {
        takingDamage = true;

        yield return new WaitForSeconds(.5f);

        takingDamage = false;
    }

    public virtual float GetHealth()
    {
        return health;
    }

    public virtual void PlayExplosionSound()
    {
        FindObjectOfType<SoundFXHandler>().Play("Explode");
    }

    public virtual void DeathBehaviour()
    {

    }
}

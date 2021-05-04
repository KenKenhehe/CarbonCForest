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

    protected float respondRange = 0;

    public Image healthBar;

    public GameObject bloodFX;
    public GameObject destoryFX;
    public GameObject[] explosionFXs;
    public float speed;
    public float maxX;
    public float minX;
    public float fallMultiplier = 2;

    public SpriteRenderer blockColorRenderer;

    public GameObject WhiteStandFX;
    public GameObject BlueStandFX;

    protected WaitForSeconds hitDuration;

    public float xSize = 2;

    public bool facingRight;
    public bool takingDamage = false;
    public bool canMove = true;

    public Color damagedColor;
    public Color originColor;

    public Animator animator;
    protected Rigidbody2D rb2d;

    protected SoundFXHandler soundFXHandler;

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
        if (playerToFocus != null)
        {
            if (playerToFocus.transform.position.x > transform.position.x)
            {
                facingRight = true;
                transform.localScale = new Vector2(-xSize, transform.localScale.y);
                if (WhiteStandFX != null && BlueStandFX != null)
                {
                    WhiteStandFX.transform.localScale = new Vector2(1, 1);
                    BlueStandFX.transform.localScale = new Vector2(1, 1);
                }
            }
            else
            {
                facingRight = false;
                transform.localScale = new Vector2(xSize, transform.localScale.y);
                if (WhiteStandFX != null && BlueStandFX != null)
                {
                    WhiteStandFX.transform.localScale = new Vector2(-1, 1);
                    BlueStandFX.transform.localScale = new Vector2(-1, 1);
                }
            }
        }
    }

    public virtual void MoveToPlayer()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.deltaTime;
        }

        if (playerToFocus.transform.position.x - respondRange > transform.position.x)
        {
            rb2d.velocity = new Vector2(speed * Time.fixedDeltaTime, rb2d.velocity.y);
        }
        else if (playerToFocus.transform.position.x + respondRange < transform.position.x)
        {
            rb2d.velocity = new Vector2(-(speed * Time.fixedDeltaTime), rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    public IEnumerator DamagedEffect()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1,0.1f);

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
        soundFXHandler.Play("Explode");
    }

    public virtual void PlaySlashSound()
    {
        soundFXHandler.Play("EnemySwordSwing");
    }

    public virtual void PlayTakeDamageSound()
    {
        soundFXHandler.Play("DamageSmall");
    }

    public virtual void DeathBehaviour()
    {

    }

    public virtual void PlayDynamicAnimation() 
    {
        if (rb2d.velocity == Vector2.zero)
        {
            animator.SetBool("IsWalking", false);
        }
        else
        {
            animator.SetBool("IsWalking", true);
        }
    }
    
    protected void CanMoveAfterShowAnimation()
    {
        StartCoroutine(WaitAndCanMove());
    }

    IEnumerator WaitAndCanMove()
    {
        canMove = false;
        yield return new WaitForSeconds(1);
        canMove = true;
    }

    public virtual void ShowEnemyCurrentStand(bool show)
    {

    }
}

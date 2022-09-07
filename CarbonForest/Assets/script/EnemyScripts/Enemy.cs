using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float unstableness = 1;
    public int minHealth = 10;
    public int maxHealth = 20;

    public int blockPoint = 2;
    public int maxBlockPoint = 3;

    [HideInInspector] public bool blocking = false;
    [HideInInspector] public bool isDead = false;


    protected PlayerAttack playerToFocus;
    protected float health;
    protected float startHealth;
    protected float SightRange;
    protected bool PlayerSeen = false;
   
    public float maxSightRange;
    public float minSightRange;

    protected float respondRange = 0;

    public GameObject AllStatusBars;

    public Image healthBar;

    public GameObject[] bloodFX;
    public GameObject destoryFX;
    public GameObject[] explosionFXs;

    public Vector3 DeathFXOffset;
    public Vector3 BloodFXOffset;

    public float speed;
    public float maxX;
    public float minX;
    public float fallMultiplier = 2;

    public SpriteRenderer blockColorRenderer;

    public GameObject WhiteStandFX;
    public GameObject BlueStandFX;
    public Vector2 StandFXSize = Vector2.one;

    protected WaitForSeconds hitDuration;

    public float xSize = 2;

    
    [HideInInspector] public bool facingRight;
    [HideInInspector] public bool takingDamage = false;
    [HideInInspector] public bool canMove = true;


    public Color damagedColor;
    public Color originColor;

    public Animator animator;
    protected Rigidbody2D rb2d;

    protected SoundFXHandler soundFXHandler;
    protected ShakeController shakeController;

    public virtual void TakeDamage(int damage)
    {
        if (AllStatusBars != null)
        {
            AllStatusBars.SetActive(true);
        }
        health -= damage;
        if (healthBar != null)
        {
            healthBar.fillAmount = health / startHealth;
        }
        if (damage < 2)
        {
            shakeController.CamShake();
            Time.timeScale = Random.Range(0.5f, 0.8f);
        }
        else if (damage <= 3)
        {
            shakeController.CamShake();
            Time.timeScale = Random.Range(0.15f, 0.3f);
        }
        else if (damage > 3)
        {
            shakeController.CamBigShake();
            Time.timeScale = Random.Range(0.05f, 0.2f);
        }

        StartCoroutine(DamagedState());
    }

    public virtual void AttackPlayer() {}

    public void FacePlayer()
    {
        if (playerToFocus != null && playerToFocus.GetComponent<PlayerGeneralHandler>().isDead == false)
        {
            if (playerToFocus.transform.position.x > transform.position.x)
            {
                facingRight = true;
                transform.localScale = new Vector2(-xSize, transform.localScale.y);
                if (WhiteStandFX != null && BlueStandFX != null)
                {
                    WhiteStandFX.transform.localScale = new Vector2(StandFXSize.x, StandFXSize.y);
                    BlueStandFX.transform.localScale = new Vector2(StandFXSize.x, StandFXSize.y);
                }
            }
            else
            {
                facingRight = false;
                transform.localScale = new Vector2(xSize, transform.localScale.y);
                if (WhiteStandFX != null && BlueStandFX != null)
                {
                    WhiteStandFX.transform.localScale = new Vector2(-StandFXSize.x, StandFXSize.y);
                    BlueStandFX.transform.localScale = new Vector2(-StandFXSize.x, StandFXSize.y);
                }
            }
        }
    }

    public void AdjustStandFXFacing()
    {
        if (playerToFocus != null && playerToFocus.GetComponent<PlayerGeneralHandler>().isDead == false)
        {
            if (playerToFocus.transform.position.x > transform.position.x)
            {
                if (WhiteStandFX != null && BlueStandFX != null)
                {
                    WhiteStandFX.transform.localScale = new Vector2(StandFXSize.x, StandFXSize.y);
                    BlueStandFX.transform.localScale = new Vector2(StandFXSize.x, StandFXSize.y);
                }
            }
            else
            {
                if (WhiteStandFX != null && BlueStandFX != null)
                {
                    WhiteStandFX.transform.localScale = new Vector2(-StandFXSize.x, StandFXSize.y);
                    BlueStandFX.transform.localScale = new Vector2(-StandFXSize.x, StandFXSize.y);
                }
            }
        }
    }

    public virtual void MoveToPlayer()
    {
        if (playerToFocus != null && playerToFocus.GetComponent<PlayerGeneralHandler>().isDead == false)
        {
            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.fixedDeltaTime;
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
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }

    }

    public IEnumerator DamagedEffect()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.1f);

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

    public virtual void PlayShowSFX()
    {
        soundFXHandler.Play("DroneShow" + Random.Range(1, 4));
    }

    public virtual void DeathBehaviour()
    {
        isDead = true;
        GameHandler.instance.globalEnemyCount -= 1;
    }

    protected void DeathWithAnimation()
    {
        AllStatusBars.SetActive(false);
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        playerToFocus.GetComponent<PlayerGeneralHandler>().RestoreHealth();
        shakeController.CamBigShake();
        if (explosionFXs != null)
        {
            Instantiate(explosionFXs[Random.Range(0, explosionFXs.Length)], transform.position + DeathFXOffset, Quaternion.identity);
        }
        animator.SetTrigger("Death" + Random.Range(1, 3));
        isDead = true;
        GameHandler.instance.globalEnemyCount -= 1;
        Destroy(gameObject, 10);
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

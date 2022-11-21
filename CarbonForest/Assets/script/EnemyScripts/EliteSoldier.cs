using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EliteSoldier : EnemyCQC
{
    enum Range
    {
        RANGE_FAR,
        RANGE_MID,
        RANGE_CLOSE
    }

    Range range;
    public float snipingRange;
    public float shootRange;
    public float meleeRange;

    public GameObject counterAttackFX;
    public float FXOffset;

    bool crouching = false;

    PlayerGeneralHandler player;
    public Transform shootTransform;
    LineRenderer lineRenderer;
    public LineRenderer SniperLine;
    public Vector3 SniperShootTransformOffset;
    LayerMask mask;

    [Header("Toggle this to enable counter attack")]
    public bool canCounterAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerGeneralHandler.instance;
        range = Range.RANGE_MID;
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(ShootWithRandomInterval());
        mask = LayerMask.GetMask("Player");
        Initialize();

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.gameObject.transform.position, transform.position);

        if (player != null)
        {
            ShooterBehaviour(distanceToPlayer);
            FacePlayer();
        }
        //print(distanceToPlayer);
        //if (player != null &&
        //    distanceToPlayer < shootRange)
        //{
        //    EnableBehaviour();
        //}
        //else
        //{
           
            
        //}
        //PlayDynamicAnimation();
    }

    public override void TakeDamage(int damage)
    {
        if (canCounterAttack)
        {
            if (blockPoint > 0)
            {
                blocking = true;
                BlockPlayer();
                health -= 1;
                UpdateHealthUI();
            }
            else
            {
                base.TakeDamage(damage);
            }
        }
        else
        {
            base.TakeDamage(damage);
        }
    }

    void BlockPlayer()
    {
        ShakeController.instance.CamShake();
        animator.SetTrigger("Counter" + Random.Range(1, 3).ToString());
        soundFXHandler.Play("SwordCling" + Random.Range(1, 5));
        Instantiate(counterAttackFX,
           new Vector3(
               (facingRight == true ? transform.position.x + FXOffset : transform.position.x - FXOffset),
               transform.position.y,
               transform.position.z),
           Quaternion.identity);

        transform.position = new Vector3(
                (facingRight ==
                true ?
                transform.position.x + (shockForce * unstableness) * 0.5f
                :
                transform.position.x - (shockForce * unstableness) * 0.5f),
                transform.position.y,
                transform.position.z);
    }

    public override void AttackPlayer()
    {
        if (canAttack == true)
        {
            if (range == Range.RANGE_CLOSE)
            {
                canMove = false;
                animator.SetTrigger("Attack");
            }
            else
                canMove = true;
        }
    }


    void ShooterBehaviour(float distanceToPlayer)
    {
        animator.SetBool("Crouching", crouching);
        if (distanceToPlayer >= shootRange)
        {
            print("In Snipe range");
            //Shoot sniping
            if (crouching == false)
            {
                animator.SetTrigger("Crouch");
                crouching = true;
            }
            range = Range.RANGE_FAR;
        }
        else if (distanceToPlayer >= meleeRange)
        {
            print("In Shoot range");
            //Shoot normal
            crouching = false;
            range = Range.RANGE_MID;
        }
        else
        {
            range = Range.RANGE_CLOSE;
            print("In melee range");
            //Melee attack
            EnableBehaviour();
        }
    }

    public void RifleShoot()
    {
        soundFXHandler.Play("RifleShoot");
        Shoot(false);
    }

    public void SniperShoot()
    {
        soundFXHandler.Play("SniperShoot");
        Shoot(true);
    }

    void Shoot(bool isSniper)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            shootTransform.position + (isSniper ? SniperShootTransformOffset : Vector3.zero),
            (facingRight ? Vector2.right : Vector2.left), 10000.0f, mask);

        if (hit.collider.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            print("Hit player");
            if (!isSniper)
            {
                lineRenderer.SetPosition(0, shootTransform.position);
                lineRenderer.SetPosition(1, hit.point + new Vector2(0, Random.Range(-.2f, .2f)));
                StartCoroutine(ShowLineRenderer());
                player.TakeEnemyDamage(1, 3, this);
            }
            else
            {
                SniperLine.SetPosition(0, shootTransform.position + SniperShootTransformOffset);
                SniperLine.SetPosition(1, hit.point + new Vector2(0, Random.Range(-.05f, .05f)));
                StartCoroutine(ShowSniperLineRenderer());
                player.TakeEnemyDamage(2, 3, this);
            }
        }
        else
        {
            if (!isSniper)
            {
                lineRenderer.SetPosition(0, shootTransform.position);
                lineRenderer.SetPosition(1, hit.point + new Vector2(0, Random.Range(-.2f, .2f)));
                StartCoroutine(ShowLineRenderer());
            }
            else
            {
                SniperLine.SetPosition(0, shootTransform.position + SniperShootTransformOffset);
                SniperLine.SetPosition(1, hit.point + new Vector2(0, Random.Range(-.05f, .05f)));
                StartCoroutine(ShowSniperLineRenderer());
            }
        }
    }

    IEnumerator ShowSniperLineRenderer()
    {
        SniperLine.enabled = true;
        yield return new WaitForSeconds(0.05f);
        SniperLine.enabled = false;
    }

    IEnumerator ShowLineRenderer()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.05f);
        lineRenderer.enabled = false;
    }

    IEnumerator ShootWithRandomInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 6));

            if (range == Range.RANGE_MID)
            {
                animator.SetBool("Shooting", true);
                yield return new WaitForSeconds(Random.Range(0.2f, 1.5f));
                animator.SetBool("Shooting", false);
            }
            else if(range == Range.RANGE_FAR)
            {
                animator.SetTrigger("CrouchShoot");
                ShakeController.instance.CamShake();
            }
        }
    }
}

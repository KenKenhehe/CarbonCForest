using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunleeController : EnemyCQC
{
    bool isOnBike = true;
    bool FallFromBike = true;
    bool isStunned = false;
    public RuntimeAnimatorController sunleeAnimator;
    public static SunleeController instance;
    SunLeeBikeController bikeController;
    [HideInInspector]
    public int fallDir = 1;

    bool falling;
    float currentFallingTime;
    public float FallTime;
    public float fallSpeed;
    public float stunTime = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        currentFallingTime = FallTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        bikeController = SunLeeBikeController.instance;
        gameObject.SetActive(false);
        Initialize();
        animator.runtimeAnimatorController = sunleeAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: if sunlee is 
        if(FallFromBike == true && isStunned == false)
        {
            //walk to bike
            MoveToBike();
        }
        else
        {
            //EnableBehaviour();
        }
    }

    void MoveToBike()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.deltaTime;
        }

        if (bikeController.transform.position.x - respondRange > transform.position.x)
        {
            rb2d.velocity = new Vector2(speed * Time.fixedDeltaTime, rb2d.velocity.y);
        }
        else if (bikeController.transform.position.x + respondRange < transform.position.x)
        {
            rb2d.velocity = new Vector2(-(speed * Time.fixedDeltaTime), rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    void Fall()
    {
        if (falling == true)
        {
            transform.localScale = new Vector2(xSize * -bikeController.currentDir, transform.localScale.y);
            if (currentFallingTime <= 0)
            {
                falling = false;
                currentFallingTime = FallTime;
                rb2d.velocity = Vector2.zero;
            }
            else
            {
                print("Falling");
                currentFallingTime -= Time.fixedDeltaTime;
                rb2d.velocity = new Vector2(fallSpeed * -bikeController.currentDir * Time.fixedDeltaTime, rb2d.velocity.y);
            }
        }
    }

    private void FixedUpdate()
    {
        Fall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<SunLeeBikeController>() != null && isStunned == false)
        {
            bikeController.GetComponent<Animator>().SetTrigger("Run");
            bikeController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            bikeController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            bikeController.setBikeMode(true);
            
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (isOnBike == true)
        {
            isStunned = true;
            transform.localScale = new Vector2(xSize * -bikeController.currentDir, transform.localScale.y);
            animator.SetTrigger("Fall");
            StartCoroutine(StunForAWhile());
        }
    }

    IEnumerator StunForAWhile()
    {
        animator.SetBool("IsStunned", isStunned);
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
        animator.SetBool("IsStunned", isStunned);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public void StopFall()
    {
        falling = false;
    }

    public void StartFall()
    {
        falling = true;
    }

}

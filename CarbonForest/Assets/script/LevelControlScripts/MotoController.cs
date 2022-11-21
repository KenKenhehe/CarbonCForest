using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MotoController : Interactable {
    public float bikeSpeed;
    Rigidbody2D rb2d;
    public float midBound;
    public float rightBound;
    public float leftBound;

    public float upBound;
    public float lowerBound;

    public Animator animator;

    bool canControl = true;

    float verticalDir = 0;
    float horizontalDir = 0;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if(SceneManager.GetActiveScene().name == "Chapter2")
        {
            animator.SetTrigger("Run");
            SoundFXHandler.instance.Play("BikeLoop");
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "Chapter1-2")
        {
            BikeInput();
        }
    }

    public void TakeDamage()
    {
        StartCoroutine(DamageEffect());
    }

    void BikeInput()
    {
        verticalDir = Input.GetAxisRaw("Vertical");
        horizontalDir = Input.GetAxisRaw("Horizontal");
        if (canControl == true)
        {
            transform.Translate(new Vector2(horizontalDir, verticalDir)* bikeSpeed * Time.deltaTime);
            if (horizontalDir == 0 && verticalDir == 0)
            {
                transform.position = Vector2.Lerp(
                        transform.position,
                        new Vector2(midBound, transform.position.y),
                        Time.deltaTime * 0.5f);
            }
            
            if(transform.position.x > rightBound)
            {
                transform.position = new Vector2(rightBound, transform.position.y);
            }
            else if (transform.position.x < leftBound)
            {
                transform.position = new Vector2(leftBound, transform.position.y);
            }

            if (transform.position.y > upBound)
            {
                transform.position = new Vector2(transform.position.x, upBound);
            }
            else if (transform.position.y < lowerBound)
            {
                transform.position = new Vector2(transform.position.x, lowerBound);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            Destroy(collision.gameObject);
            GetComponent<SceneSwitchHandler>().Interact();
            FindObjectOfType<CameraControl>().player = gameObject;
            StartCoroutine(WaitAndMoveRight());
            animator.SetTrigger("hopOn");
            SoundFXHandler.instance.Play("BikeStart");
        }
    }

    IEnumerator WaitAndMoveRight()
    {
        yield return new WaitForSeconds(4);
        rb2d.velocity = Vector2.right * bikeSpeed * Time.fixedDeltaTime;
    }

    IEnumerator DamageEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}

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
    public Animator animator;

    bool canControl = true;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if(SceneManager.GetActiveScene().name == "Chapter2")
        {
            animator.SetTrigger("Run");
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "Chapter1.5")
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
        if (canControl == true)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * bikeSpeed * Time.deltaTime);
            }
            else if(Input.GetKey(KeyCode.A)){
                transform.Translate(Vector2.left * (bikeSpeed / 2) * Time.deltaTime);
               
            }
            else
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
        }
    }

    IEnumerator WaitAndMoveRight()
    {
        yield return new WaitForSeconds(4);
        rb2d.velocity = Vector2.right * bikeSpeed * Time.deltaTime;
    }

    IEnumerator DamageEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}

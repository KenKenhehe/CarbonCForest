using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThugBombController : MonoBehaviour
{
    public Transform player;
    public int maxHeight = 5;
    public int gravity = -10;
    public int damage;
    Rigidbody2D rb2D;
    BoxCollider2D collider2D;
    bool onGround = false;
    public float initialCount = 3;
    public float explodeRadius = 2;
    public GameObject explosionFX;
    public Vector3 offset;
    public GameObject flashFX;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerGeneralHandler.instance.gameObject.transform;
        rb2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        rb2D.velocity = CalculateLaunch();
    }


    Vector3 CalculateLaunch()
    {
        float displacementY = player.position.y - transform.position.y;
        Vector3 displacementX = new Vector3(player.position.x - transform.position.x, 0, 0);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * maxHeight * gravity);
        Vector3 velocityX = displacementX / (Mathf.Sqrt(-2 * maxHeight / gravity) + Mathf.Sqrt(2 * (displacementY - maxHeight) / gravity));

        return velocityX + velocityY;

    }

    void HeadTowardTarget()
    {
        float rotateAmount = Vector3.Cross(rb2D.velocity, transform.up).z;

        rb2D.angularVelocity = -rotateAmount * 200;
    }

    private void FixedUpdate()
    {
        HeadTowardTarget();
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb2D.velocity), Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.ToLower() == "ground")
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(StartCountDown());
        }
        onGround = true;
    }

    IEnumerator StartCountDown()
    {
        while (initialCount > 0)
        {
            flashFX.SetActive(true);
            yield return new WaitForSeconds(initialCount);
            flashFX.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            initialCount -= 0.05f;
        }
        print("Explode");
        explode();
    }

    void explode()
    {
        Instantiate(explosionFX, transform.position + offset, Quaternion.identity);
        FindObjectOfType<ShakeController>().CamShake();
        if (Vector3.Distance(player.transform.position, transform.position) < explodeRadius)
        {
            player.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(damage, 3, null);
            
        }
        SoundFXHandler.instance.Play("EnemyExplode");
        Destroy(gameObject);
    }
}

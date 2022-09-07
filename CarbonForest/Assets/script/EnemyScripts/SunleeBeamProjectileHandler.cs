using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunleeBeamProjectileHandler : MonoBehaviour
{
    public int speed;
    public int damage;
    public float rotateSpeed;
    public float explodeRadius;

    public GameObject explosionFX;

    bool hasBlocked = false;
    Rigidbody2D rb2d;
    Vector2 moveDir;

    [HideInInspector]
    public int colorState;

    PlayerGeneralHandler player;

    // Start is called before the first frame update
    void Start()
    {
        moveDir = Vector2.down;
        player = PlayerGeneralHandler.instance;
        rb2d = GetComponent<Rigidbody2D>();
        SoundFXHandler.instance.Play("ProjectileIncoming");
        speed = speed + Random.Range(-400, 200);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb2d.velocity = moveDir * speed * Time.fixedDeltaTime;
    }

    void Explode()
    {
        Instantiate(explosionFX, transform.position, Quaternion.identity);

        SoundFXHandler.instance.Play("ExplodeMech");
        Destroy(gameObject);
    }

    //void HeadTowardTarget()
    //{
    //    Vector2 direction = (moveDir * 100) - rb2d.position;

    //    direction.Normalize();

    //    float rotateAmount = Vector3.Cross(direction, transform.up).z;

    //    rb2d.angularVelocity = -rotateAmount * rotateSpeed;

    //    rb2d.velocity = transform.up  * speed * Time.fixedDeltaTime;
    //}

    void changeProjectileDir()
    {
        moveDir = player.GetComponent<PlayerMovement>().facingRight ? Vector2.right : Vector2.left;
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            print("Color state: " + colorState);
            GameObject player = collision.gameObject;
            PlayerGeneralHandler playerGeneralHandler = player.GetComponent<PlayerGeneralHandler>();
            if (player.GetComponent<BlockController>().blocking == false &&
                player.GetComponent<PlayerMovement>().dodging == false)
            {
                Explode();
                player.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(damage, 0, null);
            }
            else if (player.GetComponent<BlockController>().blocking == true)
            {
                //FindObjectOfType<SoundFXHandler>().Play("SwordClingSmall");
                player.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(damage, colorState, null);

                changeProjectileDir();
                //Parry successFX
                player.GetComponent<PlayerGeneralHandler>().ParrySuccessFX();
                //
                //Todo set move towards player facing 
               
                hasBlocked = true;
            }
        }
        else if (hasBlocked)
        {
            if(collision.gameObject.GetComponent<SunleeController>() != null)
            {
                Explode();
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage * 2);
                collision.gameObject.GetComponent<SunleeController>().TakeExplosionDamage();
            }
        }
        else if(collision.gameObject.GetComponent<SunleeController>() == null && 
            collision.gameObject.tag != "LeftBound" && collision.gameObject.tag != "RightBound")
        {
            if (Vector3.Distance(player.transform.position, transform.position) < explodeRadius)
            {
                //Use 3 for color state as the color state is never 3, hence damage is always dealt
                player.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage(damage, 3, null);
            }
            Explode();
        }
    }
}

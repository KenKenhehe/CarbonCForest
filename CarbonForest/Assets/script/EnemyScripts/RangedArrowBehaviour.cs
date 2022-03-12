using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedArrowBehaviour : MonoBehaviour
{
    public float speed = 10;
    bool targetPlayer = true;
    GameObject target;
    Rigidbody2D rb;
    public float rotateSpeed = 10;
    public int Damage = 10;
    public int colorState = 0;
    public GameObject explosionFX;
    SoundFXHandler soundManager;
    [SerializeField] Vector3 explosionFXOffset;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = SoundFXHandler.instance;
        rb = GetComponent<Rigidbody2D>();
        target = PlayerGeneralHandler.instance.gameObject;
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            HeadTowardTarget();
        }
    }

    void HeadTowardTarget()
    {
        Vector2 direction = 
            ((Vector2)target.transform.position + new Vector2(Random.Range(-50f, 50f), 0)) - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount  * rotateSpeed;

        rb.velocity = transform.up * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerGeneralHandler>() != null)
        {
            print("HitPlayer");
            PlayerGeneralHandler player = collision.GetComponent<PlayerGeneralHandler>();
            player.TakeEnemyDamage(Damage, 0, new Enemy());
            Destroy(gameObject);
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            target = null;
            ArrowExplode();
            SoundFXHandler.instance.Play("ArrowHitFire");
        }
    }

    void ArrowExplode()
    {
        GameObject eFXObj = Instantiate(explosionFX, transform.position + explosionFXOffset,
            Quaternion.identity);
        Destroy(eFXObj, 1);
        Destroy(gameObject, .5f);
    }
}

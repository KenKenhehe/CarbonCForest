using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunleeBeamBehaviour : MonoBehaviour
{
    public GameObject negFX;
    public GameObject posFX;

    public GameObject explosionFX;
    public Transform explosionTransform;

    public GameObject Projectile;

    GameObject currentFX;
    int currentColorState;

    public int damage = 10;
    public float initialCount = 1;


    BoxCollider2D collider2D;
    // Start is called before the first frame update
    void Start()
    {
        negFX.SetActive(false);
        posFX.SetActive(false);
        if (Random.Range(1, 3) == 1)
        {
            negFX.SetActive(true);
            currentFX = negFX;
            currentColorState = 0;
        }
        else
        {
            posFX.SetActive(true);
            currentFX = posFX;
            currentColorState = 1;
        }

        collider2D = GetComponent<BoxCollider2D>();
        collider2D.enabled = false;

        StartCoroutine(StartFlashFX());
    }

    IEnumerator StartFlashFX()
    {
        //while (initialCount > 0.0001)
        //{
        //    currentFX.SetActive(true);
        //    //TODO Play a different sound here
        //    //SoundFXHandler.instance.Play("SpearHeavyPlasma");
        //    yield return new WaitForSeconds(initialCount);
        //    currentFX.SetActive(false);
        //    yield return new WaitForSeconds(0.1f);
        //    float secondToReduce = initialCount / 2;
        //    initialCount -= secondToReduce;
        //}

        yield return new WaitForSeconds(3);
        currentFX.SetActive(false);

        LaunchProjectile();
    }

    void LaunchProjectile()
    {
        currentFX.SetActive(false);
        GameObject projectileObj = Instantiate(Projectile, transform.position, Quaternion.identity);
        projectileObj.GetComponent<SunleeBeamProjectileHandler>().colorState = currentColorState;
        //Spawn impactFX

        //DamagePlayer
        //collider2D.enabled = true;
    }

    //void Parried()
    //{
    //    //Parry beam
    //    print("Parried");
    //    //Disable collider
    //    collider2D.enabled = false;
    //}

    //void Explode()
    //{
    //    //spawn explode FX
    //    Instantiate(explosionFX, explosionTransform.position, Quaternion.identity);

    //    //Disable collider
    //    collider2D.enabled = false;
    //    SoundFXHandler.instance.Play("ExplodeMech");
    //    Destroy(gameObject);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
    //    {
    //        print("Hit player");
    //        PlayerGeneralHandler player = collision.gameObject.GetComponent<PlayerGeneralHandler>();
    //        player.TakeEnemyDamage(damage, currentColorState, null);
    //        if (player.GetComponent<BlockController>().blocking == true)
    //        {
    //            Parried();
    //        }
    //        else
    //        {
    //            Explode();
    //        }
    //    }
    //    else
    //    {
    //        print("Hit " + collision.name);
    //        Explode();
    //    }
    //}
}

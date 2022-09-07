using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBombController : MonoBehaviour
{
    [HideInInspector] public bool onWall = false;

    public float initialCount = 3;
    public GameObject explosionFX;
    public Vector3 offset;
    public GameObject flashFX;
    BombingPointHandler BombPoint;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCountDown());
        BombPoint = GetComponentInParent<BombingPointHandler>();
    }

    IEnumerator StartCountDown()
    {
        while (initialCount > 0.0001)
        {
            flashFX.SetActive(true);
            SoundFXHandler.instance.Play("BombBeep");
            yield return new WaitForSeconds(initialCount);
            flashFX.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            float secondToReduce = initialCount / 2;
            initialCount -= secondToReduce;
        }
        print("Explode");
        explode();
    }

    void explode()
    {
        var fxRotation = onWall == false ? Quaternion.identity : Quaternion.Euler(0, 0, -90);
        Instantiate(explosionFX, transform.position + offset, fxRotation);
        FindObjectOfType<ShakeController>().CamShake();
        SoundFXHandler.instance.Play("EnemyExplode");
        BombPoint.OnExplode();
        Destroy(gameObject);
    }
}

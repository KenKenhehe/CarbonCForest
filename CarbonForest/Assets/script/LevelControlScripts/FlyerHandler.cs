using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerHandler : MonoBehaviour
{
    public float flyspeed;
    public bool facingRight = false;
    public GameObject[] enemy;

    Rigidbody2D rb2d;
    SoundFXHandler soundFX;

    Vector2 moveDirection;
    SpriteRenderer renderer;

    Vector2 MoveVector;
   

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(enemyDrop());
        renderer = GetComponent<SpriteRenderer>() == null ? GetComponentInChildren<SpriteRenderer>() :
            GetComponent<SpriteRenderer>();
        soundFX = SoundFXHandler.instance;
        rb2d = GetComponent<Rigidbody2D>();
        moveDirection = new Vector2(
            (facingRight ? 1 : -1) * flyspeed * Time.deltaTime,
            0);
        rb2d.velocity = moveDirection;
        MoveVector = new Vector2(flyspeed, 0);
        if (facingRight == true)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }
        PlayFlyBySound();
    }

    private void FixedUpdate()
    {
        rb2d.velocity = moveDirection.x * MoveVector * Time.fixedDeltaTime;
    }

    public void PlayFlyBySound()
    {
        soundFX.Play("UAVFlyBy");
    }
    
    IEnumerator enemyDrop()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].SetActive(false);
            yield return new WaitForSeconds(0.8f);
        }
    }
}

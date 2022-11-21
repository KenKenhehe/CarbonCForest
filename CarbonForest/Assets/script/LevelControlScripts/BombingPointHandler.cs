using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombingPointHandler : Interactable
{
    
    public bool onWall = false;
    bool hasCollapes = false;
    public BoxCollider2D colliderToDisable;
    public GameObject ExplosionFX;
    public GameObject bomb;
    public Transform bombTransform;
    public Sprite SpriteBroken;
    float initialCount = 2;
    bool OnTop = false;
    bool colliderDisabled = false;
    // Update is called once per frame
    void Update()
    {
        CheckIfInRange();
        EnableBehaviour();
        if(hasCollapes && OnTop && colliderDisabled == false)
        {
            colliderToDisable.enabled = false;
            colliderDisabled = true;
        }
    }

    public override void Interact()
    {
        print("INTERACT WITH BOMB POINT");
        var bombRotation = onWall ? Quaternion.Euler(0, 0, 90) : bomb.transform.rotation;
        
        //place bomb
        GameObject bombObj = Instantiate(bomb, bombTransform.position, bombRotation);

        bombObj.GetComponent<StaticBombController>().onWall = this.onWall;

        bombObj.transform.parent = this.transform;

        interacted = true;
    }

    public void OnExplode()
    {
        GetComponent<SpriteRenderer>().sprite = SpriteBroken;
        hasCollapes = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            OnTop = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            OnTop = false;
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
    //    {
    //        print("Calling On Trigger Stay");
    //        if (hasCollapes)
    //        {
    //            colliderToDisable.enabled = false;
    //            print("Now should fall");
    //        }
    //        else
    //        {
    //            print("Not falling");
    //        }
    //    }
    //}
}

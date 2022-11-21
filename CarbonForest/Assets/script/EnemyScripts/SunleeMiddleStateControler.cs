using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunleeMiddleStateControler : MonoBehaviour
{
    public void ReturnToMaimAnimation()
    {
        GetComponentInParent<SunLeeBikeController>().flipOnTurn();
        SpriteRenderer spriteRendererParent;
        spriteRendererParent = GetComponentInParent<SunLeeBikeController>().gameObject.GetComponent<SpriteRenderer>();
        spriteRendererParent.enabled = true;
        spriteRendererParent.flipX = GetComponentInParent<SunLeeBikeController>().currentDir== 1 ? true : false;
        gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().flipX = GetComponentInParent<SunLeeBikeController>().currentDir == 1 ? true : false;
        GetComponentInParent<SunLeeBikeController>().turning = false;
    }
}

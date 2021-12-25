using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerGeneralHandler>() != null)
        {
            print("Check Point Reached");

            //TODO: Enable saving icon
            //GameStateHolder.instance.FirstTimePlay = false;
            //Saver.Save(GameStateHolder.instance);
        }
    }
}

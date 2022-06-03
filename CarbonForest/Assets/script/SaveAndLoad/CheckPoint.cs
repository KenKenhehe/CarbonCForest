using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerGeneralHandler>() != null)
        {
            print("Check Point Reached");

            //TODO: Enable saving icon
            GameStateHolder.instance.FirstTimePlay = false;
            GameStateHolder.instance.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            GameStateHolder.instance.weaponCount = FindObjectOfType<PlayerAttack>().currentWeaponCount;
            Saver.Save(GameStateHolder.instance);

            Destroy(gameObject);
            //Play save anmation and sound
        }
    }
}

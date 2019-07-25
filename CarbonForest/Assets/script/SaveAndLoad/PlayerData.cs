using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int currentLevel;
    public float currentHealth;
    public int currentWeaponCount;
    public float[] position = new float[3];

    public PlayerData(PlayerGeneralHandler player)
    {
        currentHealth = player.GetHelthPoint();
        position[0] = player.gameObject.transform.position.x;
        position[1] = player.gameObject.transform.position.y;
        position[2] = player.gameObject.transform.position.z;
    }
}

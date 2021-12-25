using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Level1State
{
    public static bool SecurityDisabled;
}

[System.Serializable]
public class GameData
{
    //------Level data------
    public bool isFirstTimePlay = true;
    public int currentSceneID;

    //------Player data-----
    public float currentHealth;
    public int currentWeaponCount;
    public float[] position = new float[3];
    public int CurrentSceneIndex;

    public void SavePlayerData(PlayerGeneralHandler player)
    {
        currentHealth = player.GetHelthPoint();
        position[0] = player.gameObject.transform.position.x;
        position[1] = player.gameObject.transform.position.y;
        position[2] = player.gameObject.transform.position.z;
    }
}

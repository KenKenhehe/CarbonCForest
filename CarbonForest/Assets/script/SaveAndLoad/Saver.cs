﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public static class Saver
{
    public static void Save(GameStateHolder gameState)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GameData1.play";

        FileStream fs = new FileStream(path, FileMode.Create);

        GameData data = new GameData();
        data.CurrentSceneIndex = gameState.currentSceneIndex;
        data.isFirstTimePlay = gameState.FirstTimePlay;
        data.currentWeaponCount = gameState.weaponCount;

        formatter.Serialize(fs, data);

        fs.Close();
    }

    public static GameData Load()
    {
        string path = Application.persistentDataPath + "/GameData1.play";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            GameData data = (GameData)formatter.Deserialize(fs);
            
            fs.Close();
            Debug.Log("Progress Loaded");
            return data;
        }
        else
        {
            Debug.LogError("No data Found!");
            return null;
        }
    }
}

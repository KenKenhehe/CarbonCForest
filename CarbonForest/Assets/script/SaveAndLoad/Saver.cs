using System.Collections;
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
        string path = Application.persistentDataPath + "/GameData.play";

        FileStream fs = new FileStream(path, FileMode.Create);

        GameData data = new GameData();
        data.SavePlayerData(gameState.playerData);
        data.CurrentSceneIndex = gameState.currentSceneIndex;
        data.isFirstTimePlay = gameState.FirstTimePlay;

        formatter.Serialize(fs, data);

        fs.Close();
    }

    public static GameData Load()
    {
        string path = Application.persistentDataPath + "/GameData.play";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            GameData data = (GameData)formatter.Deserialize(fs);

            fs.Close();
            Debug.Log("Progress Saved...");
            return data;
        }
        else
        {
            Debug.LogError("No data Found!");
            return null;
        }
    }
}

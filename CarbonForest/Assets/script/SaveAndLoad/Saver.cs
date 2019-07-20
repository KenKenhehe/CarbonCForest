using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public static class Saver
{
    public static void Save(PlayerGeneralHandler player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerData.play";

        FileStream fs = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(fs, data);

        fs.Close();
    }

    public static PlayerData Load()
    {
        string path = Application.persistentDataPath + "/PlayerData.play";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            PlayerData data = (PlayerData)formatter.Deserialize(fs);

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

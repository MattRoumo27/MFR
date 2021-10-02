using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Player
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bin";
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
        }
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                PlayerData data = formatter.Deserialize(stream) as PlayerData;

                return data;
            }
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion

    #region Settings
    public static void SaveEnemies()
    {

    }

    public static void LoadEnemies()
    {

    }
    #endregion
}

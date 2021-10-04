using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Level Information

    #region SaveLevel
    public static void SaveLevel(LevelData levelData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/{levelData.sceneName}";
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, levelData);
            Debug.Log("Successfully saved to " + path);
        }
    }
    #endregion

    #region LoadLevel
    public static LevelData LoadLevel(string sceneName)
    {
        string path = Application.persistentDataPath + $"/{sceneName}";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                LevelData levelData = formatter.Deserialize(stream) as LevelData;

                return levelData;
            }
        }
        else
        {
            Debug.LogWarning("Level Save file could not be found in " + path);
            return null;
        }
    }
    #endregion

    #endregion
}

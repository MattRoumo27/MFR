using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    #region Level Information

    #region SaveLevelCollection
    public static void SaveLevelCollection(LevelCollectionData levelCollection)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levelData.bin";
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, levelCollection);
            Debug.Log("Successfully Saved Entire Level Data to " + path);
        }
    }
    #endregion

    #region LoadLevelCollection
    public static LevelCollectionData LoadLevelCollection()
    {
        string path = Application.persistentDataPath + "/levelData.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                LevelCollectionData levelCollection = formatter.Deserialize(stream) as LevelCollectionData;

                return levelCollection;
            }
        }
        else
        {
            Debug.LogWarning("Level Collection Save File Could Not Be Found in " + path);
            return null;
        }
    }
    #endregion

    #region SaveLevel
    public static void SaveLevel(LevelData levelData)
    {
        LevelCollectionData levelCollection = LoadLevelCollection();
        bool savedDataExists = levelCollection != null;

        if (savedDataExists)
        {
            levelCollection.AddOrUpdateLevel(levelData);
            SaveLevelCollection(levelCollection);
        }
        else        // Create the saved data
        {
            LevelCollectionData newLevelCollection = new LevelCollectionData();
            newLevelCollection.AddOrUpdateLevel(levelData);
            newLevelCollection.PrintLevels();
            SaveLevelCollection(newLevelCollection);
        }
    }
    #endregion

    #region LoadLevel
    public static LevelData LoadLevel(string sceneName)
    {
        LevelCollectionData levelCollection = LoadLevelCollection();
        bool savedDataExists = levelCollection != null;

        if (savedDataExists)
        {
            LevelData levelData = levelCollection.GetLevel(sceneName);
            if (levelData != null)
            {
                return levelData;
            }
        }

        return null;
    }
    #endregion

    #endregion
}

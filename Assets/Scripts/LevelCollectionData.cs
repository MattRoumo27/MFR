using System;
using System.Collections.Generic;
using UnityEngine;

#region LevelCollectionData
[Serializable]
public class LevelCollectionData
{
    private Dictionary<string, LevelData> levels;

    #region Constructor
    public LevelCollectionData()
    {
        levels = new Dictionary<string, LevelData>();
    }
    #endregion

    #region AddOrUpdateLevel
    public void AddOrUpdateLevel(LevelData levelData)
    {
        if (levels.ContainsKey(levelData.sceneName))
        {
            levels[levelData.sceneName] = levelData;
        }
        else
        {
            levels.Add(levelData.sceneName, levelData);
        }
    }
    #endregion

    #region PrintLevels
    public void PrintLevels()
    {
        foreach (KeyValuePair<string, LevelData> entry in levels)
        {
            Debug.Log(entry.Key + " exists in the dictionary");
        }
    }
    #endregion

    #region GetLevel
    public LevelData GetLevel(string levelName)
    {
        LevelData levelData;
        if (levels.TryGetValue(levelName, out levelData))
        {
            return levelData;
        }
        else
        {
            Debug.LogWarning("The level name was not in the dictionary");
            return null;
        }
    }
    #endregion
}
#endregion

#region LevelData
[Serializable]
public class LevelData {
    public string sceneName;
    public bool hasPlayerReachedCheckpoint;
    public bool hasLevelBeenCompleted;
    public CheckpointFlagData flagData;

    #region Constructor
    public LevelData(string _sceneName, bool _hasPlayerReachedCheckpoint, bool _hasLevelBeenCompleted, CheckpointFlag _flagData)
    {
        sceneName = _sceneName;
        hasPlayerReachedCheckpoint = _hasPlayerReachedCheckpoint;
        hasLevelBeenCompleted = _hasLevelBeenCompleted;
        flagData = new CheckpointFlagData(_flagData);
    }
    #endregion
}
#endregion

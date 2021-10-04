using System;

[Serializable]
public class LevelData
{
    public string sceneName;
    public bool hasPlayerReachedCheckpoint;
    public bool hasLevelBeenCompleted;
    public CheckpointFlagData flagData;

    public LevelData(string _sceneName, bool _hasPlayerReachedCheckpoint, bool _hasLevelBeenCompleted, CheckpointFlag _flagData)
    {
        sceneName = _sceneName;
        hasPlayerReachedCheckpoint = _hasPlayerReachedCheckpoint;
        hasLevelBeenCompleted = _hasLevelBeenCompleted;
        flagData = new CheckpointFlagData(_flagData);
    }
}

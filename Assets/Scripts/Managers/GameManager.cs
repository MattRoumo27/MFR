using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public const int NOT_LOADING_A_SCENE = -1;

    #region Instance
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();

            return _instance;
        }
    }
    #endregion

    #region LevelInfo
    private LevelData _levelInfo;
    public LevelData LevelInfo
    {
        get
        {
            if (_levelInfo == null)
            {
                string sceneName = SceneManager.GetActiveScene().name;
                _levelInfo = SaveManager.LoadLevel(sceneName);
            }

            return _levelInfo;
        }
    }
    #endregion

    #region PlayerReachedCheckpoint
    private bool _playerReachedCheckpoint = false;

    public bool PlayerReachedCheckpoint { 
        get 
        { 
            return _playerReachedCheckpoint; 
        } 
        set 
        { 
            if (value) 
                _playerReachedCheckpoint =  value; 
        } 
    }
    #endregion

    #region NextSceneIndex
    private int _nextSceneIndex = NOT_LOADING_A_SCENE;

    public int NextSceneIndex
    {
        get { return _nextSceneIndex; }
        set
        {
            if (value == NOT_LOADING_A_SCENE)
            {
                _nextSceneIndex = NOT_LOADING_A_SCENE;
            }
            else
            {
                Debug.LogError("NextSceneIndex was given an invalid value");
            }
        }
    }
    #endregion

    #region SetMouseCursorVisibility
    public void SetMouseCursorVisibility(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    #endregion SetMouseCursorVisiblity

    #region SaveLevelInfo
    public void SaveLevelInfo(bool isLevelCompleted)
    {
        // Gather information to prepare level for saving
        CheckpointFlag flag = Object.FindObjectOfType<CheckpointFlag>();
        bool checkpointReached = _playerReachedCheckpoint;
        string sceneName = SceneManager.GetActiveScene().name;

        if (flag != null)
        {
            LevelData levelData = new LevelData(sceneName, checkpointReached, isLevelCompleted, flag);

            if (levelData != null)
            {
                SaveManager.SaveLevel(levelData);
            }
        }
        else
        {
            Debug.LogError("Failed to get references to player and flag objects in the scene");
        }

    }
    #endregion

    #region HandleEndOfLevel
    public void HandleEndOfLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        string sceneName = scene.name;
        int levelNumber;
        string levelNumberAsString = sceneName[sceneName.Length - 1].ToString();
        bool result = int.TryParse(levelNumberAsString, out levelNumber);

        if (!result)
        {
            Debug.Log("Could not parse last character of scene name into a number: you must have finished all the levels. Well done");
        }
        else
        {
            int nextLevelNumber = levelNumber + 1;
            int nextSceneIndex = SceneUtility.GetBuildIndexByScenePath("Scenes/Level " + nextLevelNumber);
            if (nextSceneIndex >= 0)
            {
                _nextSceneIndex = nextSceneIndex;   // Triggers LoadingMenu to start loading the next scene
                ResetVariablesOnNewScene();         // Prepare for next level by resetting level specific variables
                SaveLevelInfo(true);                // Save that the level is completed
            }
            else
                Debug.LogError("The next level does not exist. You must have finished them all. Congrats!");
        }

    }
    #endregion

    #region ResetVariablesOnNewScene
    public void ResetVariablesOnNewScene()
    {
        _playerReachedCheckpoint = false;
        _levelInfo = null;
    }
    #endregion
}

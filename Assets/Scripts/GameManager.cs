using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager _instance;

    const int NOT_LOADING_A_SCENE = -1;
    private int _nextSceneIndex = NOT_LOADING_A_SCENE;

    #region NextSceneIndex
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

    #region Instance
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

    #region SavePlayerInfo
    public void SavePlayerInfo()
    {
        PlayerController player = Object.FindObjectOfType<PlayerController>();

        if (player != null)
        {
            SaveSystem.SavePlayer(player);
        }
    }
    #endregion

    #region
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
                _nextSceneIndex = nextSceneIndex;   // Triggers LoadingMenu to start loading the next scene
            else
                Debug.LogError("The next level does not exist. You must have finished them all. Congrats!");
        }

    }
    #endregion

}

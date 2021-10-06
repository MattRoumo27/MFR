using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    const int MAIN_MENU_BUILD_INDEX = 0;

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    #region Start
    private void Start()
    {
        GameManager.Instance.SetMouseCursorVisibility(false);
    }
    #endregion

    #region Update
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }
    #endregion

    #region Resume
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameManager.Instance.SetMouseCursorVisibility(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    #endregion

    #region Pause
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameManager.Instance.SetMouseCursorVisibility(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    #endregion

    #region LoadMenu
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MAIN_MENU_BUILD_INDEX);
    }
    #endregion

    #region QuitGame
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
    #endregion
}

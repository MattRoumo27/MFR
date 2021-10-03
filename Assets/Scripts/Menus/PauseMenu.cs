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

    // Update is called once per frame
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameManager.Instance.SetMouseCursorVisibility(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameManager.Instance.SetMouseCursorVisibility(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MAIN_MENU_BUILD_INDEX);
        GameManager.Instance.SavePlayerInfo();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}

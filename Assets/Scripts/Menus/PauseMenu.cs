using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public AudioClip buttonSound;
    AudioSource audioSource;

    #region Start
    private void Start()
    {
        GameManager.Instance.SetMouseCursorVisibility(false);
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Update
    void Update()
    {
        if (Input.GetButtonDown("Start") && GameManager.Instance.canPauseBeUsed)
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
        GameManager.Instance.NextSceneIndex = GameManager.MAIN_MENU_BUILD_INDEX;
        GameManager.Instance.ResetVariablesOnNewScene();
    }
    #endregion

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonSound);
    }

    #region QuitGame
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
    #endregion
}

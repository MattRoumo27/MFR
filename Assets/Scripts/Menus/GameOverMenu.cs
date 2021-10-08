using System.Collections;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject GameOverUI;

    void Update()
    {
        if (GameManager.Instance.hasPlayerDied)
        {
            ShowScreen();
        }
    }

    void ShowScreen()
    {
        GameOverUI.SetActive(true);
        GameManager.Instance.SetMouseCursorVisibility(true);
        GameManager.Instance.canPauseBeUsed = false;
    }

    #region LoadMenu
    public void LoadMenu()
    {
        GameManager.Instance.ResetVariablesOnNewScene();
        GameManager.Instance.NextSceneIndex = GameManager.MAIN_MENU_BUILD_INDEX;
    }
    #endregion

    #region RestartLevel
    public void RestartLevel()
    {
        GameManager.Instance.HandleLevelRestart();
    }
    #endregion
}

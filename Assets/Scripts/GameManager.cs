using UnityEngine;

public class GameManager
{
    private static GameManager _instance;

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
}

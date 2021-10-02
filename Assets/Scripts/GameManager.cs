using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Awake
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    #endregion

    #region Start
    private void Start()
    {
        instance.SetMouseCursorVisibility(false);
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
}

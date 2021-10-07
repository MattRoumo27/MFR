using System.Collections;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    void Update()
    {
        if (GameManager.Instance.hasPlayerDied)
        {
            // Set Game Over Screen to be active
        }
        else
        {
            // Do nothing
        }
    }
}

using UnityEngine;

public class Footsteps : MonoBehaviour
{
    Camera mainCamera;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("Can't Play Player Footsteps: AudioManager is not set up in the scene!");
        }

        AssignCameraComponent();
    }

    void AssignCameraComponent()
    {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        if (camera != null)
        {
            mainCamera = camera.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("Can't find MainCamera in scene!");
        }
    }

    // Called by Animator when the player has their foot on the ground
    public void PlayFootStepSound()
    {
        audioManager.PlayRandomFootstepSound();
    }

    public void PlayEnemyFootStepSound()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (onScreen)
            PlayFootStepSound();
    }
}

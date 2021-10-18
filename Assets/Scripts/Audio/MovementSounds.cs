using UnityEngine;

public class MovementSounds : MonoBehaviour
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

    private void AssignCameraComponent()
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

    // Called by Animators during certain keyframes
    public void PlayFootStepSound()
    {
        audioManager.PlayRandomSoundFromArray("footStepSounds");
    }

    public void PlayEnemyFootStepSound()
    {
        if (isOnScreen())
            PlayFootStepSound();
    }

    public void PlayWingFlapSound()
    {
        if (isOnScreen())
            audioManager.PlaySound("WingFlap");
    }

    private bool isOnScreen()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        return onScreen;
    }
}

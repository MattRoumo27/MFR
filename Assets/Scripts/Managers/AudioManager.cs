using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;

    [SerializeField]
    Sound[] footStepSounds;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        PopulateWithSounds(sounds, "Sound");
        PopulateWithSounds(footStepSounds, "FootstepSound");

        if (SceneManager.GetActiveScene().buildIndex == GameManager.MAIN_MENU_BUILD_INDEX)
            PlaySound("MenuMusic");
    }

    private void PopulateWithSounds(Sound[] _arrayOfSounds, string _namePrefix)
    {
        for (int i = 0; i < _arrayOfSounds.Length; i++)
        {
            string category = _namePrefix + "_" + i + "_";
            AddSoundGameObject(category, _arrayOfSounds[i]);
        }
    }

    public void AddSoundGameObject(string _name, Sound _sound)
    {
        GameObject _go = new GameObject(_name + _sound.name);
        _go.transform.SetParent(this.transform);
        _sound.SetSource(_go.AddComponent<AudioSource>());
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning("AudioManager: sound could not be found in list, " + _name);
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        Debug.LogWarning("AudioManager: sound could not be found in list, " + _name);
    }

    // Given a list of names: play one of the footstep sounds randomly
    public void PlayRandomFootstepSound()
    {
        int randomIndex = Random.Range(0, footStepSounds.Length);
        footStepSounds[randomIndex].Play();
    }
}
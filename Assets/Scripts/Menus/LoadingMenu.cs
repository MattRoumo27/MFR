using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingMenu : MonoBehaviour
{
    Slider _slider;
    TextMeshProUGUI _progressText;

    void Start()
    {
        _slider = GetComponentInChildren<Slider>();
        _progressText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        int nextScene = GameManager.Instance.NextSceneIndex;

        if (nextScene != -1)
        {
            StartCoroutine(LoadAsynchronously(nextScene));
        }
    }

    public IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        const string loadingUIName = "LoadingScreen";
        GameObject loadingUI = transform.Find(loadingUIName).gameObject;

        if (loadingUI != null)
        {
            loadingUI.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);

                if (_slider != null && _progressText != null)
                {
                    _slider.value = progress;
                    _progressText.text = progress * 100f + "%";
                }
                else
                {
                    GameManager.Instance.NextSceneIndex = -1;
                }

                yield return null;
            }

            GameManager.Instance.NextSceneIndex = -1;
        }
        else
        {
            Debug.LogError("Could not find " + loadingUIName + " in this gameObject");
        }
    }
}

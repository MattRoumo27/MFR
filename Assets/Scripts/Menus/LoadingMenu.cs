using System.Collections;
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

        if (nextScene != GameManager.NOT_LOADING_A_SCENE)
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
                    GameManager.Instance.NextSceneIndex = GameManager.NOT_LOADING_A_SCENE;
                }

                yield return null;
            }

            GameManager.Instance.NextSceneIndex = GameManager.NOT_LOADING_A_SCENE;
        }
        else
        {
            Debug.LogError("Could not find " + loadingUIName + " in this gameObject");
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject levelsPlaceholder;
    public GameObject levelCardPrefab;
    public GameObject SelectLevelMenu;
    public Slider slider;
    public TextMeshProUGUI progressText;
    public AudioClip buttonSound;

    AudioManager audioManager;

    [SerializeField]
    string hoverOverSound = "ButtonHover";
    [SerializeField]
    string buttonClickSound = "ButtonClick";

    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No audiomanager found!");
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        SelectLevelMenu.SetActive(false);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void CreateLevelButtons()
    {
        for (int levelNumber = 1; levelNumber < SceneManager.sceneCountInBuildSettings; levelNumber++)
        {
            GameObject levelCard = Instantiate(levelCardPrefab, levelsPlaceholder.transform);
            RectTransform cardRect = (RectTransform)levelCard.transform;

            const float padding = 50;
            float spacingWidth = cardRect.rect.width + padding;

            levelCard.transform.SetParent(levelsPlaceholder.transform);
            levelCard.transform.position = levelCard.transform.position + new Vector3((levelNumber - 1) * spacingWidth, 0, 0);

            Button cardButton = levelCard.GetComponent<Button>();
            TextMeshProUGUI cardText = levelCard.GetComponentInChildren<TextMeshProUGUI>();
            cardText.text = $"Level {levelNumber}";

            int buttonSceneIndex = levelNumber;
            cardButton.onClick.AddListener(() => LoadLevel(buttonSceneIndex));
            cardButton.onClick.AddListener(() => PlayButtonClickSound());
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayButtonClickSound()
    {
        audioManager.PlaySound(buttonClickSound);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverOverSound);
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}

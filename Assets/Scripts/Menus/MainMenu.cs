using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject levelsReferencePoint;
    public GameObject levelCardPrefab;
    public GameObject selectLevelMenu;
    public Slider loadProgress;
    public TextMeshProUGUI progressText;

    AudioManager audioManager;

    [SerializeField]
    string hoverOverSound = "ButtonHover";
    [SerializeField]
    string buttonClickSound = "ButtonClick";

    void Start()
    {
        ConfigureAudioManager();
    }

    void ConfigureAudioManager()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager found!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region Audio
    public void PlayButtonClickSound()
    {
        audioManager.PlaySound(buttonClickSound);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverOverSound);
    }
    #endregion

    #region Loading
    public void LoadLevel(int sceneIndex)
    {
        selectLevelMenu.SetActive(false);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            loadProgress.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
    #endregion

    #region Levels
    public void CreateLevelButtons()
    {
        for (int levelNumber = 1; levelNumber < SceneManager.sceneCountInBuildSettings; levelNumber++)
        {
            GameObject levelCard = Instantiate(levelCardPrefab, levelsReferencePoint.transform);
            SetLevelCardParentAndPosition(levelCard, levelNumber);
            ConfigureLevelCardButton(levelCard, levelNumber);
            ConfigureEventTriggerForLevelCard(levelCard);
        }
    }

    private void SetLevelCardParentAndPosition(GameObject levelCard, int levelNumber)
    {
        RectTransform cardRect = (RectTransform)levelCard.transform;

        const float padding = 50;
        float spacingWidth = cardRect.rect.width + padding;

        levelCard.transform.SetParent(levelsReferencePoint.transform);
        levelCard.transform.position = levelCard.transform.position + new Vector3((levelNumber - 1) * spacingWidth, 0, 0);
    }

    private void ConfigureLevelCardButton(GameObject levelCard, int levelNumber)
    {
        Button cardButton = levelCard.GetComponent<Button>();
        TextMeshProUGUI cardText = levelCard.GetComponentInChildren<TextMeshProUGUI>();
        cardText.text = $"Level {levelNumber}";

        AddListeners(cardButton, levelNumber);
    }

    private void AddListeners(Button cardButton, int levelNumber)
    {
        int levelSceneIndex = levelNumber;
        cardButton.onClick.AddListener(() => LoadLevel(levelSceneIndex));
        cardButton.onClick.AddListener(() => PlayButtonClickSound());
    }

    private void ConfigureEventTriggerForLevelCard(GameObject levelCard)
    {
        EventTrigger.Entry eventType = new EventTrigger.Entry();
        eventType.eventID = EventTriggerType.PointerEnter;
        eventType.callback.AddListener((eventData) => { OnMouseOver(); });

        EventTrigger cardTrigger = levelCard.AddComponent<EventTrigger>();
        cardTrigger.triggers.Add(eventType);
    }
    #endregion
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{

    [Tooltip("The game version string.")]
    public string version;
    public bool stable;
    public TextMeshProUGUI versionText;
    public Button StartButton;
    public Button SettingsButton;
    public Button CreditsButton;
    public Button QuitButton;

    public InGameMenuManager m_PauseMenu;
    private EventSystem es;

    void Start()
    {
        versionText.text = "Version: " + version + (stable ? " (Stable)" : " (Unstable)");
        StartButton.onClick.AddListener(OnStartButtonClicked);
        SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        CreditsButton.onClick.AddListener(OnCreditsButtonClicked);
#if UNITY_WEBGL
        QuitButton.gameObject.SetActive(false);
#else
        QuitButton.onClick.AddListener(OnQuitButtonClicked);
#endif
        es = EventSystem.current;
    }

    void Update()
    {
        if (Input.GetButtonDown("Vertical")) OnNavigate();
    }

    void OnNavigate()
    {
        if (!es.currentSelectedGameObject)
        {
            es.SetSelectedGameObject(StartButton.gameObject);
        }
    }

    public void OnHighlightButton(GameObject obj)
    {
        es.SetSelectedGameObject(obj);
    }
    void OnStartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnSettingsButtonClicked()
    {
        if (!m_PauseMenu.gameObject.activeSelf) m_PauseMenu.SetPauseMenuActivation(true);
    }

    void OnCreditsButtonClicked()
    {
        print("Show Credits");
    }

    void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(0);
#endif
    }
}

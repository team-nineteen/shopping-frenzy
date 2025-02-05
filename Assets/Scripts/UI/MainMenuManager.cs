﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainMenuManager : MonoBehaviour
{

    private string version;
    private bool stable;

    [Header("Version")]
    public TextAsset versionFile;
    public TextMeshProUGUI versionText;

    [Header("Buttons")]
    public Button StartButton;
    public Button SettingsButton;
    public Button CreditsButton;
    public Button QuitButton;
    public InGameMenuManager m_PauseMenu;
    private EventSystem es;

    private const string STABLE_NUM = ".0123456789";

    void Start()
    {
        version = versionFile.text.Trim();
        stable = IsStable(version);
        versionText.text = "Version: " + version + (stable ? "" : " (Unstable)");
        StartButton.onClick.AddListener(OnStartButtonClicked);
        SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        CreditsButton.onClick.AddListener(OnCreditsButtonClicked);
#if UNITY_WEBGL
        QuitButton.gameObject.SetActive(false);
#else
        QuitButton.onClick.AddListener(OnQuitButtonClicked);
#endif
        es = EventSystem.current;
        Time.timeScale = 1.0f;
    }

    static bool IsStable(string version)
    {
        foreach (char c in version)
            if (!STABLE_NUM.Contains("" + c))
                return false;
        return true;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + (SettingsData.Instance.skipIntro ? 2 : 1));
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

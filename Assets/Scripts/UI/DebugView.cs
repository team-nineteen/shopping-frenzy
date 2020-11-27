using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugView : MonoBehaviour
{
    [Tooltip("The root of all text nodes.")]
    public GameObject root;

    [Header("FPS")]
    [Tooltip("The text field displaying the framerate")]
    public TextMeshProUGUI fpsText;
    [Tooltip("Delay between updates of the displayed framerate value")]
    public float pollingTime = 0.5f;
    
    [Header("Goals")]
    [Tooltip("The text field displaying the moneySpentGoal")]
    public TextMeshProUGUI moneyGoalText;
    [Tooltip("The text field displaying the timeSpentGoal")]
    public TextMeshProUGUI timeGoalText;

    [Tooltip("The text field displaying the current seed")]
    public TextMeshProUGUI seedText;
    SettingsData m_SettingsData;
    float m_AccumulatedDeltaTime = 0f;
    int m_AccumulatedFrameCount = 0;

    void Start()
    {
        m_SettingsData = SettingsData.Instance;

        root.gameObject.SetActive(m_SettingsData.debugEnabled);

        m_SettingsData.onDebugChanged += OnDebugChanged;
    }

    public void SetDebugGoals(string ms, string ts) {
        moneyGoalText.text = "Money Goal: " + ms;
        timeGoalText.text = "Time Goal: " + ts;
    }

    public void SetSeedText(int seed) {
        seedText.text = "Seed: " + seed;
    }

    void OnDebugChanged(bool newState)
    {
        root.SetActive(newState);
    }
    void Update()
    {
        if (Input.GetButtonDown(GameConstants.k_ButtonDebugView))
        {
            m_SettingsData.debugEnabled = !m_SettingsData.debugEnabled;
        }

        if (m_SettingsData.debugEnabled)
        {
            m_AccumulatedDeltaTime += Time.deltaTime;
            m_AccumulatedFrameCount++;

            if (m_AccumulatedDeltaTime >= pollingTime)
            {
                int framerate = Mathf.RoundToInt((float)m_AccumulatedFrameCount / m_AccumulatedDeltaTime);
                fpsText.text = framerate.ToString();

                m_AccumulatedDeltaTime = 0f;
                m_AccumulatedFrameCount = 0;
            }
        }
    }
}

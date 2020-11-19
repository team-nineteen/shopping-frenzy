using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugView : MonoBehaviour
{
    [Header("FPS")]
    [Tooltip("The text field displaying the framerate")]
    public TextMeshProUGUI fpsText;
    [Tooltip("Delay between updates of the displayed framerate value")]
    public float pollingTime = 0.5f;

    SettingsData m_SettingsData;
    float m_AccumulatedDeltaTime = 0f;
    int m_AccumulatedFrameCount = 0;
    void Start()
    {
        m_SettingsData = FindObjectOfType<SettingsData>();
        DebugUtility.HandleErrorIfNullFindObject<SettingsData, MenuManager>(m_SettingsData, this);

        fpsText.gameObject.SetActive(m_SettingsData.debugEnabled);

        m_SettingsData.onDebugChanged += OnDebugChanged;
    }

    void OnDebugChanged(bool newState)
    {
        fpsText.gameObject.SetActive(newState);
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

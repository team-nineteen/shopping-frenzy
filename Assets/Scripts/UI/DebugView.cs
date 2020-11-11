using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugView : MonoBehaviour
{
    public bool debugEnabled = false;

    [Header("FPS")]
    [Tooltip("The text field displaying the framerate")]
    public TextMeshProUGUI fpsText;
    [Tooltip("Delay between updates of the displayed framerate value")]
    public float pollingTime = 0.5f;

    float m_AccumulatedDeltaTime = 0f;
    int m_AccumulatedFrameCount = 0;
    void Start()
    {
        fpsText.gameObject.SetActive(debugEnabled);
    }
    void Update()
    {
        if (Input.GetButtonDown(GameConstants.k_ButtonDebugView))
        {
            debugEnabled = !debugEnabled;
            fpsText.gameObject.SetActive(debugEnabled);
        }

        if (debugEnabled)
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

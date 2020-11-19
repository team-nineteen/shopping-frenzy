using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    const string SFX_STRING = "SFXVolume";
    const string MUS_STRING = "MusicVolume";
    public AudioMixer[] audioMixers;
    SettingsData m_SettingsData;

    void Start()
    {
        m_SettingsData = FindObjectOfType<SettingsData>();
        DebugUtility.HandleErrorIfNullFindObject<SettingsData, AudioManager>(m_SettingsData, this);

        m_SettingsData.onSfxVolumeChanged += OnSfxVolumeChanged;
        m_SettingsData.onMusicVolumeChanged += OnMusicVolumeChanged;
    }

    void OnSfxVolumeChanged(float val)
    {
        SetFloat(SFX_STRING, val);
    }

    void OnMusicVolumeChanged(float val)
    {
        SetFloat(MUS_STRING, val);
    }

    public AudioMixerGroup[] FindMatchingGroups(string subPath)
    {
        for (int i = 0; i < audioMixers.Length; i++)
        {
            AudioMixerGroup[] results = audioMixers[i].FindMatchingGroups(subPath);
            if (results != null && results.Length != 0)
            {
                return results;
            }
        }

        return null;
    }

    public void SetFloat(string name, float value)
    {
        for (int i = 0; i < audioMixers.Length; i++)
        {
            if (audioMixers[i] != null)
            {
                audioMixers[i].SetFloat(name, Mathf.Log10(value) * 20);
            }
        }
    }

    public void GetFloatAsDB(string name, out float value)
    {
        value = 0f;
        for (int i = 0; i < audioMixers.Length; i++)
        {
            if (audioMixers[i] != null)
            {
                audioMixers[i].GetFloat(name, out value);
                break;
            }
        }
    }

    public float GetFloat(string name)
    {
        float value = 0f;
        for (int i = 0; i < audioMixers.Length; i++)
        {
            if (audioMixers[i] != null)
            {
                audioMixers[i].GetFloat(name, out value);
                break;
            }
        }
        return Mathf.Pow(10f, (value / 20f));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsData : ScriptableObject
{
    private static SettingsData _instance;

    public static SettingsData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<SettingsData>();
                _instance.SetDefaults();
            }
            return _instance;
        }
    }

    public Score highScore { get; set; }
    public UnityAction<float> onFovChanged;
    public UnityAction<bool> onDebugChanged;
    public UnityAction<float> onSfxVolumeChanged;
    public UnityAction<float> onMusicVolumeChanged;

    public void ClearSubscriptions()
    {
        onFovChanged = null;
        onDebugChanged = null;
    }
    void SetDefaults()
    {
        highScore = null;
        mouseSensitivity = 1.0f;
        fov = 70.0f;
        sfxVolume = 0.5f;
        musicVolume = 0.5f;
        toggleCrouch = false;
        toggleSprint = false;
        toggleInteract = false;
        debugEnabled = false;
        hintsEnabled = true;
        skipIntro = false;
    }

    private float _mouseSensitivity;
    public float mouseSensitivity
    {
        get
        {
            return _mouseSensitivity;
        }
        set
        {
            _mouseSensitivity = value;
        }
    }
    private float _fov;
    public float fov
    {
        get
        {
            return _fov;
        }
        set
        {
            _fov = value;
            if (onFovChanged != null) onFovChanged.Invoke(_fov);
        }
    }
    private float _sfxVolume;
    public float sfxVolume
    {
        get
        {
            return _sfxVolume;
        }
        set
        {
            _sfxVolume = value;
            if (onSfxVolumeChanged != null) onSfxVolumeChanged.Invoke(_sfxVolume);
        }
    }
    private float _musicVolume;
    public float musicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            _musicVolume = value;
            if (onMusicVolumeChanged != null) onMusicVolumeChanged.Invoke(_musicVolume);
        }
    }
    private bool _toggleSprint;
    public bool toggleSprint
    {
        get
        {
            return _toggleSprint;
        }
        set
        {
            _toggleSprint = value;
        }
    }
    private bool _toggleCrouch;
    public bool toggleCrouch
    {
        get
        {
            return _toggleCrouch;
        }
        set
        {
            _toggleCrouch = value;
        }
    }
    private bool _toggleInteract;
    public bool toggleInteract
    {
        get
        {
            return _toggleInteract;
        }
        set
        {
            _toggleInteract = value;
        }
    }
    private bool _debugEnabled;
    public bool debugEnabled
    {
        get
        {
            return _debugEnabled;
        }
        set
        {
            _debugEnabled = value;
            if (onDebugChanged != null) onDebugChanged.Invoke(_debugEnabled);
        }
    }

    private bool _hintsEnabled;
    public bool hintsEnabled
    {
        get
        {
            return _hintsEnabled;
        }
        set
        {
            _hintsEnabled = value;
        }
    }
    private bool _skipIntro;
    public bool skipIntro
    {
        get
        {
            return _skipIntro;
        }
        set
        {
            _skipIntro = value;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Handles Crosshair switching.
 */
public class Crosshair : MonoBehaviour
{
    GameObject m_AimDefault;
    GameObject m_AimHover;
    GameObject m_AimHold;

    public Aim CurrentAim {get; private set;}

    public enum Aim {
        None,
        Default,
        Hover,
        Hold
    }
    
    // Changes the current crosshair style to another one.
    public void ChangeTo(Aim a) {
        if (a == CurrentAim) return;
        m_AimDefault.SetActive(a == Aim.Default);
        m_AimHover.SetActive(a == Aim.Hover);
        m_AimHold.SetActive(a == Aim.Hold);
        CurrentAim = a;
    }

    void Start() {
        m_AimDefault = transform.Find("Aim_Default").gameObject;
        m_AimHover = transform.Find("Aim_Hover").gameObject;
        m_AimHold = transform.Find("Aim_Hold").gameObject;

        ChangeTo(Aim.Default);
    }

    void Update() { }
}

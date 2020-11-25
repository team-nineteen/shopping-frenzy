using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    void Start()
    {
        AudioSource[] AudioSources = GetComponents<AudioSource>();
        int i = Random.Range(0, AudioSources.Length);
        AudioSources[i].Play();
    }
}

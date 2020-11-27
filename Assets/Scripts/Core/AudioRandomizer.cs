using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{

    private AudioSource currentlyPlaying;
    private AudioSource[] AudioSources;

    //private bool randomizeOnEnd; // not implemented: If another song should be chosen instead of the same one.
    void Start()
    {
        RandomizeSong();
    }

    public void RandomizeSong()
    {
        if (currentlyPlaying) Stop();
        AudioSources = GetComponents<AudioSource>();
        int i = Random.Range(0, AudioSources.Length);
        currentlyPlaying = AudioSources[i];
        currentlyPlaying.Play();
    }

    public void Stop()
    {
        currentlyPlaying.Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class fence_open : MonoBehaviour
{
    public float speed = 1f;
    bool isOpening = false;
    bool isClosing = false;
    bool wait = false;
    float timer;
    float timerLength = 1f;

    public Transform door;

    public Collider hitbox;
    public AudioSource sound;

    private Vector3 spawnPos;


    // Start is called before the first frame update
    void Start()
    {
        spawnPos = door.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening && timer > 0f)
        {
            door.Rotate(Vector3.up, Time.deltaTime *speed);
            timer -= Time.deltaTime;
        }
        
        if (isOpening && timer <= 0f) 
        {
            isOpening = false;
            wait = true;
            timer = timerLength;
        }

        if (wait && timer > 0f)
        {
            timer -= Time.deltaTime;
        }


        if (wait && timer <= 0f)
        {
            wait = false;
            isClosing = true;
            timer = timerLength;
        }


        if (isClosing && timer > 0f)
        {
            door.Rotate(Vector3.down * Time.deltaTime * speed);
            timer -= Time.deltaTime;
        }
        if (isClosing && timer <= 0f)
        {
            isClosing = false;
            door.position = spawnPos;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (!isOpening)
        {
            if (!isClosing)
            {
                if (!wait)
                {
                    isOpening = true;
                    timer = timerLength;
                    if (sound) sound.Play();
                }
            }
        }
    }
}
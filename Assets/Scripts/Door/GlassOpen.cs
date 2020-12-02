using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GlassOpen : MonoBehaviour
{
    private Vector3 spawnPosR;
    private Vector3 spawnPosL;
    public float speed = 1f;
    bool isOpening = false;
    bool isClosing = false;
    bool wait = false;
    float timer;
    float timerLength = 1f;
    float gracePeriod = 1;

    public Transform doorLeft;
    public Transform doorRight;

    public Collider hitbox;
    public AudioSource sound;


    // Start is called before the first frame update
    void Start()
    {
        spawnPosR = doorRight.position;
        spawnPosL = doorLeft.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gracePeriod > 0) gracePeriod -= Time.deltaTime;
        if (isOpening && timer > 0f)
        {
            doorLeft.Translate(Vector3.right * Time.deltaTime *speed);
            doorRight.Translate(Vector3.left * Time.deltaTime * speed);
            timer -= Time.deltaTime;
        }
        
        if (isOpening && timer <= 0f) 
        {
            isOpening = false;
            wait = true;
            timer = timerLength *2;
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
            doorLeft.Translate(Vector3.left * Time.deltaTime * speed);
            doorRight.Translate(Vector3.right * Time.deltaTime * speed);
            timer -= Time.deltaTime;
        }
        if (isClosing && timer <= 0f)
        {
            doorRight.position = spawnPosR;
            doorLeft.position = spawnPosL;
            isClosing = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (gracePeriod > 0) return;
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
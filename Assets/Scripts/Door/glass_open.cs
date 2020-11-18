using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class glass_open : MonoBehaviour
{
    public float speed = 1f;
    bool isOpening = false;
    bool isClosing = false;
    bool wait = false;
    float timer;
    float timerLength = 1f;

    public Transform doorLeft;
    public Transform doorRight;

    public Collider hitbox;
    public AudioSource sound;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
            isClosing = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (!isOpening)
        {
            isOpening = true;
            timer = timerLength;
            if (sound) sound.Play();
        }
    }
}
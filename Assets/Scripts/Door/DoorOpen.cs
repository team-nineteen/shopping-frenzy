using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public float speed = 1f;
    bool isOpening = false;
    bool isClosing = false;
    float timer;
    float timerLength = 2f;
    float gracePeriod = 1;

    public Transform door;

    public Collider hitbox;
    public AudioSource sound;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gracePeriod > 0) gracePeriod -= Time.deltaTime;
        if (isOpening && timer > 0f)
        {

            door.Translate(Vector3.up * Time.deltaTime * speed);
            timer -= Time.deltaTime;
        }
        if (isOpening && timer <= 0f)
        {
            isOpening = false;
            isClosing = true;
            timer = timerLength;
        }
        if (isClosing && timer > 0f)
        {
            door.Translate(Vector3.down * Time.deltaTime * speed);
            timer -= Time.deltaTime;
        }
        if (isClosing && timer <= 0f)
        {
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
                isOpening = true;
                timer = timerLength;
                if (sound) sound.Play();
            }
        }
    }
}
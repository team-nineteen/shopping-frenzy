using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayGate : MonoBehaviour
{

    public Collider turnoff;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GroceryCart>())
        {
            turnoff.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GroceryCart>())
        {
            turnoff.gameObject.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        GroceryCart gc = other.GetComponent<GroceryCart>();
        if (gc) {
            gc.OnShopLeave(); // Whenever your cart enters the triggerzone, your stuff gets purchased.
        }
    }

    void OnTriggerExit(Collider other) {
        
    }
}

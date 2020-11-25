using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryCart : MonoBehaviour
{

    private GroceryList groceryList;
    
    void Start() {
        groceryList = GameObject.Find("GameManager").GetComponent<GroceryList>();
    }

    public void OnShopLeave() {
        groceryList.PurchaseAll();
    }

    void OnTriggerExit(Collider other) {
        Item item = other.gameObject.GetComponent<Item>();
        if (item) {
            OnCartLeave(item);
        }
    }
    void OnTriggerEnter(Collider other) {
        Item item = other.gameObject.GetComponent<Item>();
        if (item) {
            OnCartEnter(item);
        }
    }
    void OnCartEnter(Item item) {
        if (item && item.gameObject.activeSelf) {
            groceryList.Attach(item);
        }
    }

    void OnCartLeave(Item item) {
        if (item && item.gameObject.activeSelf) {
            groceryList.Dettach(item);
        }
    }
}

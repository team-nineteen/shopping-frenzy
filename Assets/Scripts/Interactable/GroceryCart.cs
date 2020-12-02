using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryCart : MonoBehaviour
{
    [Tooltip("Sound to play whenever you purchase stuff.")]
    public AudioSource purchaseSound;
    private GroceryList groceryList;
    private List<Item> items;

    void Start()
    {
        groceryList = GameObject.Find("GameManager").GetComponent<GroceryList>();
        items = new List<Item>();
    }

    public void OnShopLeave()
    {
        bool boughtSomething = items.Count > 0;
        groceryList.PurchaseAll(items);
        if (boughtSomething && purchaseSound) purchaseSound.Play();
        items.Clear();
    }

    void OnTriggerExit(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();
        if (item)
        {
            OnCartLeave(item);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();
        if (item)
        {
            OnCartEnter(item);
        }
    }
    void OnCartEnter(Item item)
    {
        groceryList.Attach(item);
        items.Add(item);
    }

    void OnCartLeave(Item item)
    {
        groceryList.Dettach(item);
        items.Remove(item);
    }
}

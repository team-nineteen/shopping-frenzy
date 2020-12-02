using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Holdable
{
    public enum GroceryType
    {
        NONE = 0,
        VEGGIE = 1,     // Fruits and vegetables
        BREAD = 2,      // Bread types
        BAKERY = 4,     // Fresh sandwiches, donuts etc.
        INEDIBLE = 8,   // Bag of dirt
        DANGEROUS = 16, // Soap, cleaning
        COOLED = 32,    // Cheese
        FRESH = 64,     // So called fresh salad and prepared meals
        FROZEN = 128,   // Beef
        MILK = 256,     // Milk
        MISC = 512,     // Anything else
        BACK = 1024,    // Things in back area.
        STACKED = 2048  // Items that cn be stacked, like TP or cans.
    }

    [Tooltip("Purely used to link to an internal item ID (Should be set in prefab and never be changed).")]
    public string registeredName;
    [Tooltip("How much percent off an item is.")]
    [Range(0.0f, 1.0f)]
    public float discountFactor = 0.0f;
    public int discountAmount = 0;
    public int priceInCents { get { return (int)(info.basePriceInCents * (1f - discountFactor) - discountAmount); } }
    private ItemInfo info;
    public bool paid { get; set; }
    public int id { get { return info.id; } }
    public GroceryType groceryGroup;

    protected override void Start()
    {
        base.Start();
        info = ItemRegistry.GetByName(registeredName);
    }
}

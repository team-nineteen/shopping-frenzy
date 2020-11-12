using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Holdable
{

    [Tooltip("Purely used to link to an internal item ID (Should be set in prefab and never be changed).")]
    public string registeredName;
    [Tooltip("How much percent off an item is.")]
    [Range(0.0f, 1.0f)]
    public float discountFactor = 0.0f;
    public int discountAmount = 0;
    public int priceInCents { get { return (int)(info.basePriceInCents * (1f - discountFactor) - discountAmount); } }
    private ItemInfo info;
    public bool paid {get; set;}
    public bool inCart {get; set;}
    public int id { get { return info.id; } }

    protected override void Start()
    {
        base.Start();
        info = ItemRegistry.GetByName(registeredName);
    }
}

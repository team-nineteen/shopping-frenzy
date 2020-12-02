using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocationData : MonoBehaviour
{
    

    [Tooltip("Maximum allowed deviation from center. (In plane rectangular space)")]
    public float deviationRadius = 0f;
    [Tooltip("If the rotation should be copied from the spawn space's world rotation.")]
    public bool copyRotation = true;
    [Tooltip("Maximum times this location can be used to spawn items after which it is removed.")]
    public int uses = 1;
    [Tooltip("How likely this spawning space will be used. Higher is more likely.")]
    public float weight = 1f;
    [Tooltip("What types of groceries may spawn.")]

    public Item.GroceryType[] groceryGroups;
    private Item.GroceryType combinedGroceryGroup = 0; // Flag enum type.

    void Start()
    {
        foreach (Item.GroceryType t in groceryGroups) combinedGroceryGroup |= t;
    }

}

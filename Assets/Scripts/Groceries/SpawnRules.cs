using System.Collections;
using System.Collections.Generic;
using static System.Enum;
using UnityEngine;

public class SpawnRules : MonoBehaviour
{

    [Tooltip("The root object containing all valid spawn locations")]
    public Transform spawnLocationsRoot;
    [Tooltip("List of all grocery prefabs")]
    public Item[] groceries;
    [Tooltip("How many to spawn of each grocery")]
    public int[] counts;

    public bool useSeed = false;
    public int seed = 0;

    private Dictionary<Item.GroceryType, List<SpawnLocationData>> groceryGroups;

    void Start()
    {
        if (groceries.Length != counts.Length) throw new UnityException("Groceries and Counts MUST match!");
        groceryGroups = new Dictionary<Item.GroceryType, List<SpawnLocationData>>();
        foreach (Item.GroceryType t in System.Enum.GetValues(typeof(Item.GroceryType)))
        {
            groceryGroups.Add(t, new List<SpawnLocationData>());
        }
        if (useSeed) Random.InitState(seed);
        InitializeSpawnLocations(spawnLocationsRoot);

        SpawnGroceries();
    }

    void InitializeSpawnLocations(Transform root)
    {
        SpawnLocationData data = root.GetComponent<SpawnLocationData>();
        if (data && data.uses > 0 && data.weight > 0)
        {
            foreach (Item.GroceryType t in data.groceryGroups)
            {
                for (int i = 0; i < data.weight; i++)
                {
                    groceryGroups[t].Add(data); // Add this object to lists for each weight.
                }
            }
        }
        foreach (Transform t in root)
        {
            InitializeSpawnLocations(t);
        }
    }

    void SpawnGroceries()
    {
        int[] shuffledIndices = new int[groceries.Length];
        for (int i = 0; i < groceries.Length; i++) shuffledIndices[i] = i;
        Shuffle(shuffledIndices, 10);

        foreach (int i in shuffledIndices)
        {
            Item groceryItem = groceries[i];
            int max = counts[i];
            SpawnGrocery(groceryItem, max);
        }
    }

    void Shuffle(int[] shuffledIndices, int times)
    {
        for (int i = 0; i < times; i++)
        {
            for (int n = shuffledIndices.Length - 1; n > 0; n--)
            {
                int r = Random.Range(1, n);
                int t = shuffledIndices[r];
                shuffledIndices[r] = shuffledIndices[n];
                shuffledIndices[n] = t;
            }
        }
    }


    void SpawnGrocery(Item groceryItem, int count)
    {
        int spawned = 0;
        while (spawned < count)
        {
            // Determine category and location
            List<SpawnLocationData> groupLocations = groceryGroups[groceryItem.groceryGroup];
            if (groupLocations.Count < 1) throw new UnityException("No spawning spaces left!"); // TODO, make enough spaces or select from a backup spawning space e.g. the floor.
            int r = Random.Range(0, groupLocations.Count);
            SpawnLocationData location = groupLocations[r];

            while (location.uses > 0 && spawned < count)
            {
                // Instantiate objects
                Quaternion rotation = location.copyRotation ? location.transform.rotation : Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                Vector3 center = location.transform.position;
                Vector3 position = location.deviationRadius == 0 ? center : RandomlyDeviatedVector3(center, location.deviationRadius);
                Instantiate(groceryItem, position, rotation);

                // Decrement and remove location if used up.
                location.uses--; spawned++;
            }
            groupLocations.RemoveAt(r);
        }
    }

    Vector3 RandomlyDeviatedVector3(Vector3 center, float d)
    {
        return new Vector3(Random.Range(center.x - d, center.x + d), center.y, Random.Range(center.z - d, center.z + d));
    }
}

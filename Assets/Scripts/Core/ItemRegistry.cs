using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRegistry
{

    private static List<ItemInfo> idRegistry;
    private static Dictionary<string, ItemInfo> nameRegistry;

    // First item ID is 1 (0 is reserved for empty), Last item ID is size
    public static int size {get; private set;}

    // ANY ITEM THAT CAN APPEAR ON THE Grocery List SHOULD BE REGISTERED! Just copy a line with Register(++size, <Name>, <Price>); and modify it to your needs!
    static ItemRegistry()
    {
        idRegistry = new List<ItemInfo>();
        nameRegistry = new Dictionary<string, ItemInfo>();

        size = 0;
        Register(0, "None", 0);
        Register(++size, "Apple", 60);
        Register(++size, "Milk", 120);
        Register(++size, "Bread", 100);
        Register(++size, "Meat", 800);
    }

    private static void Register(int id, string name, int basePriceInCents)
    {
        ItemInfo item = new ItemInfo(id, name, basePriceInCents);
        idRegistry.Add(item);
        nameRegistry.Add(name, item);
    }

    public static List<ItemInfo> GetList() {
        return idRegistry;
    }
    public static ItemInfo GetById(int id)
    {
        return idRegistry[id];
    }

    public static ItemInfo GetByName(string name)
    {
        return nameRegistry[name];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public int id { get; private set; }
    public string displayName { get; private set; }
    public string registeredName { get; private set; }
    public int basePriceInCents { get; private set; }
    public ItemInfo(int id, string registeredName, int basePriceInCents, string displayName)
    {
        this.id = id;
        this.displayName = displayName;
        this.registeredName = registeredName;
        this.basePriceInCents = basePriceInCents;
    }

    public override bool Equals(object obj)
    {
        return !(obj == null || GetType() != obj.GetType()) && ((obj as ItemInfo).id == id);
    }

    public override int GetHashCode()
    {
        return id;
    }
}

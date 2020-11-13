using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GroceryList : MonoBehaviour
{
    private List<GroceryItem> items;
    private int maxGroceries;

    [Tooltip("The transform that will contain the grocery list items.")]
    public Transform groceryList;
    public TextMeshProUGUI spentText;
    public int moneySpentInCents { get; set; }

    private const string spentTextFormat = "Total Spent: €{0},{1:D2}";
    void Start()
    {
        spentText.text = string.Format(spentTextFormat, 0, 0);
        items = new List<GroceryItem>();
        maxGroceries = groceryList.childCount;
        for (int i = 0; i < maxGroceries; i++)
        {
            var child = groceryList.GetChild(i);
            child.gameObject.SetActive(false);
        }
        InitializeList();
    }

    void InitializeList()
    {
        List<ItemInfo> itemListCopy = new List<ItemInfo>();
        itemListCopy.AddRange(ItemRegistry.GetList());

        for (int i = 0; i < maxGroceries; i++)
        {
            int r = itemListCopy.Count < 2 ? 0 : Random.Range(1, itemListCopy.Count); // Throw empty when all have been selected.
            ItemInfo item = itemListCopy[r];
            items.Add(new GroceryItem(item));
            if (r > 0) itemListCopy.RemoveAt(r);

            var child = groceryList.GetChild(i);
            var text = child.GetComponent<TextMeshProUGUI>();
            text.text = item.displayName;
            text.color = Color.white;
            text.fontStyle = FontStyles.Bold;
            child.gameObject.SetActive(true);
        }
    }

    TextMeshProUGUI GetTextComponent(Item item)
    {
        int i = items.FindIndex(x => x.id == item.id);
        if (i < 0) return null;
        var child = groceryList.GetChild(i);
        return child.GetComponent<TextMeshProUGUI>();
    }
    void Purchase(Item item)
    {
        if (item && item.gameObject.activeSelf && !item.paid)
        {
            moneySpentInCents += item.priceInCents;
            item.paid = true;
            OnUpdate(null, item);
            item.gameObject.SetActive(false); // Remove from cart
        }
    }
    public void PurchaseAll()
    {
        foreach (var listItem in items)
        {
            foreach (var item in listItem.items)
            {
                Purchase(item);
            }
        }
        spentText.text = string.Format(spentTextFormat, moneySpentInCents / 100, moneySpentInCents % 100);
    }

    public void Attach(Item item)
    {
        GroceryItem gi = items.Find(i => (i != null) && (i.id == item.id));
        if (gi != null)
        {
            gi.items.Add(item); // Bind this current item.
            OnUpdate(gi, item);
        }
    }

    public void Dettach(Item item)
    {
        GroceryItem gi = items.Find(i => (i != null) && (i.id == item.id));
        if (gi != null)
        {
            gi.items.Remove(item);
            OnUpdate(gi, item);
        }
    }

    void OnUpdate(GroceryItem gi, Item item)
    {
        var text = GetTextComponent(item);
        if (text == null) // Nonlist items are not displayed.
            return;
        if (item.paid)
        {
            text.color = Color.grey;
            text.fontStyle = FontStyles.Strikethrough;
        }
        else
        {
            if (gi == null || text.color == Color.grey) return;
            if (gi.items.Count > 0) // Got at least 1 of these items.
            {
                text.color = Color.cyan;
                text.fontStyle = FontStyles.Normal;
            }
            else
            {
                text.color = Color.white;
                text.fontStyle = FontStyles.Bold;
            }
        }
    }

    private class GroceryItem
    {
        private ItemInfo itemInfo;
        public List<Item> items;
        public int id { get { return itemInfo.id; } }

        public GroceryItem(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
            items = new List<Item>();
        }
    }

}

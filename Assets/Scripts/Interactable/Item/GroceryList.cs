using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GroceryList : MonoBehaviour
{

    [Header("Winning Goals")]
    [Tooltip("Max time allowed to spend in seconds.")]
    public int timeGoalInSeconds;
    public int moneySpentGoalInCents { get; set; } // Automatically calculated based on price of all grocery items.

    private List<GroceryItem> items;
    private int maxGroceries;

    [Tooltip("The transform that will contain the grocery list items.")]
    public Transform groceryList;
    public TextMeshProUGUI moneySpentText;
    public TextMeshProUGUI timeSpentText;
    private int moneySpentInCents;
    private int timeSpentInSeconds;
    private float floatSeconds;
    private DebugView m_DebugView;
    private WinMenuManager m_WinMenu;
    private Score score;
    private int acquiredItemCount;

    void Start()
    {
        score = FindObjectOfType<Score>();
        floatSeconds = 0;
        m_DebugView = FindObjectOfType<DebugView>();
        m_WinMenu = FindObjectOfType<WinMenuManager>();

        moneySpentText.text = "Money Spent: " + Score.MoneyString(0);
        timeSpentText.text = "Time Spent: " + Score.TimeString(0);

        items = new List<GroceryItem>();
        maxGroceries = groceryList.childCount;
        for (int i = 0; i < maxGroceries; i++)
        {
            var child = groceryList.GetChild(i);
            child.gameObject.SetActive(false);
        }
        InitializeList();

        m_DebugView.SetDebugGoals(Score.MoneyString(moneySpentGoalInCents), Score.TimeString(timeGoalInSeconds));
    }



    void Update()
    {
        if (Time.timeScale > 0) // TimeScale == 0 -> game paused
        {
            floatSeconds += Time.deltaTime;
            int newTime = (int)floatSeconds;
            if (newTime > timeSpentInSeconds)
            {
                timeSpentInSeconds = newTime;
                timeSpentText.text = "Time Spent: " + Score.TimeString(timeSpentInSeconds);
            }
        }
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

            moneySpentGoalInCents += item.basePriceInCents; // Total goal
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
        if (item && !item.paid)
        {
            int i = items.FindIndex(x => x.id == item.id);
            if (i >= 0 && !items[i].obtained)
            {
                items[i].obtained = true;
                acquiredItemCount++;
            }
            moneySpentInCents += item.priceInCents;
            item.paid = true;
            OnUpdate(null, item);
            Destroy(item.gameObject); // Remove from cart
        }
    }
    public void PurchaseAll(List<Item> itemList)
    {
        foreach (var item in itemList)
        {
            Purchase(item);
        }
        moneySpentText.text = "Money Spent: " + Score.MoneyString(moneySpentInCents);
        if (acquiredItemCount >= maxGroceries)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        score.timeInSecondsSpent = timeSpentInSeconds;
        score.moneySpent = moneySpentInCents;
        score.CalculateScore(moneySpentGoalInCents, timeGoalInSeconds);
        SettingsData.Instance.ClearSubscriptions();
        m_WinMenu.OnActivate(score);
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
        public bool obtained = false;
        public List<Item> items;
        public int id { get { return itemInfo.id; } }

        public GroceryItem(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
            items = new List<Item>();
        }
    }

}

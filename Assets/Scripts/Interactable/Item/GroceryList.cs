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
    [Tooltip("Money Spent Text")]
    public TextMeshProUGUI moneySpentText;
    [Tooltip("Time Spent Text")]
    public TextMeshProUGUI timeSpentText;

    [Header("Grocery List")]
    [Tooltip("The rendering texture of the grocery list.")]
    public Transform textureRoot;
    [Tooltip("List of sprites signifying bought status")]
    public Transform boughtStatusList;
    [Tooltip("List of sprites signifying in-cart status")]
    public Transform inCartStatusList;
    [Tooltip("List of Registered items for Items in Scene")]
    public Transform itemRegistryList;
    [Tooltip("List of Display Slots for Items in Scene")]
    public Transform displayItemList;
    private const int maxGroceries = 9;
    private int moneySpentInCents;
    private int timeSpentInSeconds;
    private float floatSeconds;
    private DebugView m_DebugView;
    private WinMenuManager m_WinMenu;
    private Score score;
    private int acquiredItemCount;

    enum ItemState
    {
        None,
        Unobtained,
        InCart,
        Bought
    }

    private Dictionary<int, int> itemIndexMap;
    private Dictionary<int, GroceryItem> groceryItemIdMap;

    void Start()
    {
        floatSeconds = 0;
        score = FindObjectOfType<Score>();
        m_DebugView = FindObjectOfType<DebugView>();
        m_WinMenu = FindObjectOfType<WinMenuManager>();

        moneySpentText.text = "Money Spent: " + Score.MoneyString(0);
        timeSpentText.text = "Time Spent: " + Score.TimeString(0);

        itemIndexMap = new Dictionary<int, int>();
        groceryItemIdMap = new Dictionary<int, GroceryItem>();
        InitializeList();
        m_DebugView.SetDebugGoals(Score.MoneyString(moneySpentGoalInCents), Score.TimeString(timeGoalInSeconds));
    }

    private void SetTextColor(TextMeshProUGUI txt, int spent, int goal, float margin = 1.5f)
    {
        if (spent < goal) return;
        if (spent == goal)
            txt.color = Color.green;
        else if (spent < goal * margin)
            txt.color = Color.yellow;
        else
            txt.color = Color.red;
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
                SetTextColor(timeSpentText, timeSpentInSeconds, timeGoalInSeconds);
            }
            AnimateGroceriesInCart();
        }
    }
    void AnimateGroceriesInCart()
    {
        foreach (int id in groceryItemIdMap.Keys)
            if (groceryItemIdMap[id].state == ItemState.InCart)
                displayItemList.GetChild(itemIndexMap[id]).Rotate(Vector3.up, Time.deltaTime * 90);
    }
    private void InitializeList()
    {
        for (int i = 0; i < maxGroceries; i++)
        {
            int r = itemRegistryList.childCount < 2 ? 0 : Random.Range(1, itemRegistryList.childCount);
            Transform displayItem = itemRegistryList.GetChild(r);
            Item item = displayItem.GetComponent<Item>();
            ItemInfo itemInfo = ItemRegistry.GetByName(item.registeredName);
            GroceryItem gi = new GroceryItem(itemInfo);
            itemIndexMap.Add(gi.id, i);
            groceryItemIdMap.Add(gi.id, gi);
            if (r > 0)
            {
                displayItem.SetParent(displayItemList.GetChild(i));
                displayItem.localPosition = Vector2.zero;
                displayItem.localRotation = Quaternion.identity;
                SetItemState(gi.id, ItemState.Unobtained);
            }
            moneySpentGoalInCents += itemInfo.basePriceInCents; // Total goal
        }
    }

    // Enable: True to gray out item.
    private void GrayOut(int id, bool enable)
    {
        Transform disp = displayItemList.GetChild(itemIndexMap[id]);
        disp.localPosition = new Vector3(disp.localPosition.x, disp.localPosition.y, enable ? 15 : 5);
    }

    private void SetItemState(int itemId, ItemState newState)
    {
        int i = itemIndexMap[itemId];
        GrayOut(itemId, newState == ItemState.Bought);
        inCartStatusList.GetChild(i).gameObject.SetActive(newState == ItemState.InCart);
        boughtStatusList.GetChild(i).gameObject.SetActive(newState == ItemState.Bought);
        groceryItemIdMap[itemId].state = newState;
    }

    private void Purchase(Item item)
    {
        if (item && !item.paid)
        {
            if (groceryItemIdMap.TryGetValue(item.id, out GroceryItem gi) && gi.state != ItemState.Bought)
            {
                acquiredItemCount++;
                SetItemState(item.id, ItemState.Bought);
            }
            moneySpentInCents += item.priceInCents;
            item.paid = true;
            Destroy(item.gameObject); // Remove from cart
        }
    }
    public void PurchaseAll(List<Item> itemList)
    {
        foreach (var item in itemList) Purchase(item);
        moneySpentText.text = "Money Spent: " + Score.MoneyString(moneySpentInCents);
        SetTextColor(moneySpentText, moneySpentInCents, moneySpentGoalInCents);
        if (acquiredItemCount >= maxGroceries) EndGame();
    }

    private void EndGame()
    {
        score.timeInSecondsSpent = timeSpentInSeconds;
        score.moneySpent = moneySpentInCents;
        score.CalculateScore(moneySpentGoalInCents, timeGoalInSeconds);
        if (SettingsData.Instance.highScore == null || SettingsData.Instance.highScore.score < score.score) {
            SettingsData.Instance.highScore = score;
        }
        SettingsData.Instance.ClearSubscriptions();
        m_WinMenu.OnActivate(score);
    }

    public void Attach(Item item)
    {
        if (item != null && groceryItemIdMap.TryGetValue(item.id, out GroceryItem gi))
        {
            gi.items.Add(item); // Bind this current item.
            if (gi.state != ItemState.Bought && gi.items.Count == 1) // If this is the first item attached of this type.
                SetItemState(item.id, ItemState.InCart);
        }
    }

    public void Dettach(Item item)
    {
        if (item != null && groceryItemIdMap.TryGetValue(item.id, out GroceryItem gi))
        {
            gi.items.Remove(item);
            if (gi.state != ItemState.Bought && gi.items.Count == 0) // If this is the last item attached of this type.
                SetItemState(item.id, ItemState.Unobtained);
        }
    }

    private class GroceryItem
    {
        private ItemInfo itemInfo;
        public ItemState state;
        public List<Item> items;
        public int id { get { return itemInfo.id; } }

        public GroceryItem(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
            state = itemInfo.id == 0 ? ItemState.None : ItemState.Unobtained;
            items = new List<Item>();
        }
    }

}

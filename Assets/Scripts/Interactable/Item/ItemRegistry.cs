﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRegistry
{

    private static List<ItemInfo> idRegistry;
    private static Dictionary<string, ItemInfo> nameRegistry;

    // First item ID is 1 (0 is reserved for empty), Last item ID is size
    public static int size {get; private set;}

    // ANY ITEM THAT CAN APPEAR ON THE Grocery List SHOULD BE REGISTERED! Just copy a line with Register(++size, <RegisteredName>, <Price>, [<DisplayName>]);
    // and modify it to your needs! The display is optional and will be the same as registered name if not set.
    static ItemRegistry()
    {
        idRegistry = new List<ItemInfo>();
        nameRegistry = new Dictionary<string, ItemInfo>();

        size = 0;
        Register(0, "None", 0, "-");
        Register(++size, "Apple", 60);
        Register(++size, "Avocado", 80);
        Register(++size, "Banana", 70);
        Register(++size, "Basil", 100, "Basil Plant");
        Register(++size, "BeerBoxBlue", 270, "Cheap Beer Box");
        Register(++size, "BeerBoxGreen", 300, "Regular Beer Box");
        Register(++size, "BeerBoxRed", 350, "Premium Beer Box");
        Register(++size, "Bread", 100);
        Register(++size, "Broccoli", 110);
        Register(++size, "CanBlack", 100, "Cola Zeerow Can");
        Register(++size, "CanGreen", 120, "Furnandez Lime Can");
        Register(++size, "CanOrange", 90, "Orange Juice Can");
        Register(++size, "CanRed", 110, "Cola Can");
        Register(++size, "Carrot", 60);
        Register(++size, "CheeseDark", 270, "Matured Cheese");
        Register(++size, "CheeseLight", 220, "Young Cheese");
        Register(++size, "ChipsGreen", 130, "Prungles Bolognese");
        Register(++size, "ChipsRed", 100, "Prungles Original");
        Register(++size, "ChipsWhite", 120, "Prungles Onion");
        Register(++size, "Cucumber", 80);
        Register(++size, "DoughBlue", 110, "Natural Pizza Dough");
        Register(++size, "DoughGreen", 140, "Vegan Pizza Dough");
        Register(++size, "JoghurtBlue", 130, "Blueberry Joghurt");
        Register(++size, "JoghurtGreen", 120, "Regular Joghurt");
        Register(++size, "JoghurtYellow", 130, "Vanilla Joghurt");
        Register(++size, "Leek", 90);
        Register(++size, "Lemon", 40);
        Register(++size, "Meat", 500);
        Register(++size, "MenuBowl", 150, "Prepared Dinner");
        Register(++size, "Milk", 120);
        Register(++size, "Mozzarella", 100);
        Register(++size, "Mushroom", 20);
        Register(++size, "Onion", 30);
        Register(++size, "Pear", 60);
        Register(++size, "Pepper", 120);
        Register(++size, "PetCola", 130, "Bottle of Cola");
        Register(++size, "PetFanta", 110, "Bottle of Oranta");
        Register(++size, "PetWater", 80, "Bottle of Water");
        Register(++size, "SaladBowl", 120, "Fresh Salad");
        Register(++size, "Sandwich", 140, "Fresh Sandwich");
        Register(++size, "SoapBottle", 150, "Bottle of Soap");
        Register(++size, "SoilBag", 90, "Bag of Dirt");
        Register(++size, "SprayBottle", 180, "Cleaning Spray");
        Register(++size, "StrawberryPlant", 130, "Strawberry Plant");
        Register(++size, "ToiletPaper", 40, "Toilet Paper");
        Register(++size, "Tomato", 50);
    }

    private static void Register(int id, string registeredName, int basePriceInCents, string displayName = null)
    {
        if (displayName == null) displayName = registeredName;
        ItemInfo item = new ItemInfo(id, registeredName, basePriceInCents, displayName);
        idRegistry.Add(item);
        nameRegistry.Add(registeredName, item);
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

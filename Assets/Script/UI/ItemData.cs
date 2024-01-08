using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemData
{
    public int itemId;
    public string itemName;
    public int itemPrice;
    public int stack;
    public bool stackableItem;
    public int itemSizeX;
    public int itemSizeY;
    public string itemDescription;
}

public struct InventoryData
{
    public ItemData itemData;
    public int posX;
    public int posY;
    public bool isRotated;
}
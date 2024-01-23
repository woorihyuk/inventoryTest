using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[Serializable]
public class ItemData
{
    public int itemId;
    public string itemName;
    public int itemPrice;
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
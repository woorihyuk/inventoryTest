using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ItemData
{
    [FormerlySerializedAs("itemId")] public int id;
    public string itemName;
    public int itemPrice;
    public bool stackableItem;
    public int itemSizeX;
    public int itemSizeY;
    public string itemDescription;
}

[Serializable]
public class GunData
{
    public int damage;
    //public int ammoType
    public int magazine;
}

[Serializable]
public class CloseRangeWeaponData
{
    public int damage;
}

[Serializable]
public class ArmorData
{
    public float armorPoint;
}

[Serializable]
public struct InInventoryItemData
{
    public ItemData data;
    public int posX;
    public int posY;
    public int stack;
    public bool isRotated;
}
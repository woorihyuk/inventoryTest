using System;

public enum EquipmentType
{
    Default = 1,
    Available = 2,
    Gun = 3,
    Weapon = 4,
    Armor = 5,
    HadGear = 6,
    Bag = 7
}

[Serializable]
public class ItemData
{
    public int id;
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
public class WeaponData
{
    public int damage;
}

[Serializable]
public class ArmorData
{
    public float armorPoint;
}

[Serializable]
public class BagData
{
    public int sizeX;
    public int sizeY;
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
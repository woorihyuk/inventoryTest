using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Player
{
    public enum ItemType
    {
        
    }
    public class InventoryManager : MonoBehaviour
    {
<<<<<<< HEAD
        public InInventoryItem[] inventoryContents;
        public GameObject[] inventoryGrid;
        public void AddItem(InInventoryItem inInventoryItem)
=======
        public Item[] inventoryContents;

        public void AddItem(Item item)
>>>>>>> parent of d1c1c3c (인벤토리 ui)
        {
            
        }
    }
}
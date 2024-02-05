using System.Collections.Generic;
using Script.UI;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        private List<InInventoryItemData> _inventoryContents;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            instance = this;
        }

        public void SaveItem(List<InInventoryItemData> items)
        {
            _inventoryContents = items;
        }

        public List<InInventoryItemData> LoadItem()
        {
            return _inventoryContents;
        }
        
    }
}
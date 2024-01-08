using System;
using System.Collections.Generic;
using Script.Object;
using Script.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Player
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        public List<DefaultItem> inInventoryItems;
        public GameObject inventory;
        public ItemBox openedBox;
        public DefaultItem grabbedItem;

        private void Awake()
        {
            Instance = this;
            print("dfdf");
        }

        public void AddItem(DefaultItem defaultItem)
        {
            inInventoryItems.Add(defaultItem);
        }
    }
}


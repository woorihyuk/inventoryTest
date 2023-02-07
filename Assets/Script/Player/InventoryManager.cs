using System;
using System.Collections.Generic;
using Script.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Player
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        public InInventoryItem[] inventoryContents;
        public RectTransform[] inventoryGrid;
        public GameObject inventory;

        private void Awake()
        {
            Instance = this;
        }

        public void AddItem(InInventoryItem inInventoryItem)
        {
            
        }
    }
}
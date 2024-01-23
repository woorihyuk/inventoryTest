using System;
using System.Collections.Generic;
using Script.Object;
using Script.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Script.Player
{
    public struct InventorySizeData
    
    
    {
        public int sizeX;
        public int sizeY;
    }
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        public DefaultItem[,] inInventoryItems;
        
        public InGameUiManager inGameUiManager;
        
        //플레이어 인벤토리 사이즈
        public InventorySizeData inventorySizeData;
        //인벤토리 그리드
        public bool[,] inventoryGrids;
        //인벤토리 업데이트 여부
        public bool isInventoryUpdate;
        
        public GameObject inventory;
        public ItemBox openedBox;
        public DefaultItem grabbedItem;

        private void Awake()
        {
            Instance = this;
            
        }

        public void OpenInventory()
        {
            inventory.SetActive(true);
            if (isInventoryUpdate)
            {
                //inventoryGrids = new bool[inventorySizeData.sizeX, inventorySizeData.sizeY];
                //inGameUiManager.InventoryUpdate(inventorySizeData.sizeX, inventorySizeData.sizeY);
                inventoryGrids = new bool[5, 5];
                inInventoryItems = new DefaultItem[5, 5];
                inGameUiManager.InventoryUpdate(5, 5);
                isInventoryUpdate = false;
            }

            inGameUiManager.InventoryOnOff(true);
        }

        public void CloseInventory()
        {
            inventory.SetActive(false);
            inGameUiManager.InventoryOnOff(false);
        }
        
        public void SaveInventory()
        {
           
        }
    }
}


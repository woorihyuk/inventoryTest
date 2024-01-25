using System;
using System.Collections.Generic;
using System.Linq;
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
        public static InventoryManager instance;

        private InInventoryItemData[,] _inInventoryItemData;
        
        public InGameUiManager inGameUiManager;
        
        //플레이어 인벤토리 사이즈
        public InventorySizeData inventorySizeData;
        //인벤토리 그리드
        public bool[,] inventoryOverlapInfo;
        //인벤토리 업데이트 여부
        public bool isInventoryUpdate;
        public bool isBoxOpened;
        
        public ItemBox openedBox;
        public DefaultItem grabbedItem;
        
        [SerializeField] private GameObject inventory;
        
        private void Awake()
        {
            instance = this;
            
        }

        public void OpenInventory()
        {
            inventory.SetActive(true);
            if (isInventoryUpdate)
            {
                //inventoryGrids = new bool[inventorySizeData.sizeX, inventorySizeData.sizeY];
                //inGameUiManager.InventoryUpdate(inventorySizeData.sizeX, inventorySizeData.sizeY);
                inventoryOverlapInfo = new bool[5, 5];
                _inInventoryItemData = new InInventoryItemData[5, 5];
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

        public InInventoryItemData GetItemData(int x, int y)
        {
            return _inInventoryItemData[x, y];
        }

        public void AddToInventory(InInventoryItemData item)
        {
            _inInventoryItemData[item.posX, item.posY] = item;
        }
        
        public void RemoveFromInventory(InInventoryItemData data)
        {
            _inInventoryItemData[data.posX, data.posY].data = null;

        }
    }
}


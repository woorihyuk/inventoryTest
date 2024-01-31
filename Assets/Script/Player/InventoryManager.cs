using System;
using System.Collections.Generic;
using System.Linq;
using Script.Object;
using Script.UI;
using UnityEngine;
using UnityEngine.InputSystem;
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
        
        public InGameUiManager inGameUiManager;
        private InputAction _openInventoryAction;
        private InputAction _closeInventoryAction;
        private InputAction _itemRotateAction;
        
        private InInventoryItemData[,] _inInventoryItemData;

        //플레이어 인벤토리 사이즈
        public InventorySizeData inventorySizeData;
        //인벤토리 그리드
        public bool[,] inventoryOverlapInfo;
        //인벤토리 업데이트 여부
        public bool isInventoryUpdate;
        
        public ItemBox openedBox;
        public DefaultItem grabbedItem;
        
        [SerializeField] private GameObject inventory;
        
        private void Awake()
        {
            instance = this;
            _openInventoryAction = InputController.instance.inputActionAsset.FindAction("OpenInventory");
            _closeInventoryAction = InputController.instance.inputActionAsset.FindAction("Escape");
            _itemRotateAction = InputController.instance.inputActionAsset.FindAction("ItemRotate");
            _openInventoryAction.performed += _ => OpenInventory();
            _closeInventoryAction.performed += _ => CloseInventory();
            _itemRotateAction.performed += _ => RotateItem();
        }

        public void OpenInventory()
        {
            InputController.instance.inputActionAsset.FindActionMap("Player").Disable();
            InputController.instance.inputActionAsset.FindActionMap("UiInput").Enable();
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

        private void CloseInventory()
        {
            InputController.instance.inputActionAsset.FindActionMap("Player").Enable();
            InputController.instance.inputActionAsset.FindActionMap("UiInput").Disable();
            inventory.SetActive(false);
            if (openedBox != null)
            {
                inGameUiManager.RootingMenuOff();
                openedBox.RemoveBox();
            }
            inGameUiManager.InventoryOnOff(false);
        }
        
        public void GrabItem(DefaultItem item)
        {
            grabbedItem = item;
        }
        
        
        public void DropItem()
        {
            grabbedItem = null;
        }

        public InInventoryItemData GetItemData(int x, int y)
        {
            return _inInventoryItemData[x, y];
        }

        public void AddToInventory(InInventoryItemData item, RectTransform rectTransform)
        {
            _inInventoryItemData[item.posX, item.posY] = item;
            rectTransform.SetParent(inGameUiManager.inInventoryItemParent);
        }
        
        public void RemoveFromInventory(InInventoryItemData data)
        {
            _inInventoryItemData[data.posX, data.posY].data = null;

        }

        private void RotateItem()
        {
            if (grabbedItem == null)
            {
                return;
            }
            grabbedItem.RotateItem();
        }
    }
}


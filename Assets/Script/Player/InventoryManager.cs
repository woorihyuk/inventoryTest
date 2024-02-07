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
    
    public abstract class EquipmentSlot<T>
    {
        public bool isUsed;
        public InInventoryItemData slotItem;
        public T equipmentData;
    }
    
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager instance;
        
        [FormerlySerializedAs("inGameUiManager")] public GridManager gridManager;

        private InputActionMap _playerActionMap;
        private InputActionMap _uiActionMap;
        
        private InputAction _openInventoryAction;
        private InputAction _closeInventoryAction;
        private InputAction _itemRotateAction;
        
        private InInventoryItemData[,] _inInventoryItemData;
        private List<InInventoryItemData> _inInventoryItemList;

        // 장비 슬롯
        public EquipmentSlot<GunData> gunData;

        public EquipmentSlot<WeaponData> weaponData;

        public EquipmentSlot<ArmorData> armorData;
        
        public EquipmentSlot<HeadGearData> headGearData;

        public EquipmentSlot<BagData> bagData;
        
        // 인벤토리 그리드
        public bool[,] inventoryOverlapInfo;
        // 인벤토리 업데이트 여부
        public bool isInventoryUpdate;
        
        public ItemBox openedBox;
        public DefaultItem grabbedItem;
        
        [SerializeField] private GameObject inventory;

        private bool _isGrab;
        
        public InInventoryItemData GetItemData(int x, int y)
        {
            return _inInventoryItemData[x, y];
        }
        
        private void Awake()
        {
            instance = this;
            _playerActionMap = InputController.instance.inputActionAsset.FindActionMap("Player");
            _uiActionMap = InputController.instance.inputActionAsset.FindActionMap("UiInput");
            _openInventoryAction = InputController.instance.inputActionAsset.FindAction("OpenInventory");
            _closeInventoryAction = InputController.instance.inputActionAsset.FindAction("Escape");
            _itemRotateAction = InputController.instance.inputActionAsset.FindAction("ItemRotate");
            _openInventoryAction.performed += _ => OpenInventory();
            _closeInventoryAction.performed += _ => CloseInventory();
            _itemRotateAction.performed += _ => RotateItem();
        }

        public void OpenInventory()
        {
            _playerActionMap.Disable();
            _uiActionMap.Enable();
            inventory.SetActive(true);
            if (isInventoryUpdate)
            {
                //inventoryGrids = new bool[inventorySizeData.sizeX, inventorySizeData.sizeY];
                //inGameUiManager.InventoryUpdate(inventorySizeData.equipmentData.sizeX, inventorySizeData.equipmentData.sizeY);
                inventoryOverlapInfo = new bool[5, 5];
                _inInventoryItemData = new InInventoryItemData[5, 5];
                gridManager.InventoryUpdate(5, 5);
                isInventoryUpdate = false;
            }

            gridManager.InventoryOnOff(true);
        }

        private void CloseInventory()
        {
            _playerActionMap.Enable();
            _uiActionMap.Disable();
            inventory.SetActive(false);
            if (openedBox != null)
            {
                gridManager.RootingMenuOff();
                openedBox.RemoveBox();
            }
            gridManager.InventoryOnOff(false);
        }
        
        public void GrabItem()
        {
            _isGrab = true;
        }
        
        public void DropItem()
        {
            _isGrab = false;
        }

        public void AddToInventory(InInventoryItemData data, RectTransform rectTransform)
        {
            _inInventoryItemData[data.posX, data.posY] = data;
            rectTransform.SetParent(gridManager.inInventoryItemParent);
            _inInventoryItemList.Add(data);
        }
        
        public void RemoveFromInventory(InInventoryItemData data)
        {
            _inInventoryItemData[data.posX, data.posY].data = null;
            _inInventoryItemList.Remove(data);

        }

        private void RotateItem()
        {
            if (!_isGrab)
            {
                return;
            }
            grabbedItem.RotateItem();
        }
    }
}


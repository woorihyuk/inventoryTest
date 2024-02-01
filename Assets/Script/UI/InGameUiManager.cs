using System;
using System.Collections.Generic;
using Script.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public enum Equipment
{
    Bag,
    Gun,
    Weapon,
    Armor,
    HadGear,
}

namespace Script.UI
{
    public class InGameUiManager : MonoBehaviour
    {
        // 이거 ui  출력 싸개로 변경하기!!!
        public GameObject[] inGameUi;
        public TMP_Text boxName;
        
        public RectTransform[,] inventoryGridObjects;
        public RectTransform[,] itemBoxGridObjects;
        
        public GameObject inventoryGrid;
        
        // 인벤토리, 박스 위치
        public RectTransform[] equipments;
        public RectTransform inventory;
        public RectTransform itemBox;
        
        // 그리드 부모 오브젝트
        public RectTransform inventoryGridParent;
        public RectTransform itemBoxGridParent;

        // 아이템 부모 오브젝트
        public RectTransform inInventoryItemParent;
        public RectTransform inBoxItemParent;
        
        private IObjectPool<RectTransform> _gridObjectPool;
        
        private int _itemGridCount;

        private void Start()
        {
            // items = inGameUi[3].GetComponent<RectTransform>();
            // inventorySizeX = 5;
            // InventoryGrid = new ItemGrid[5, inventorySizeY];
            // itemBoxGrid = new ItemGrid[5, 8];
            
            _gridObjectPool = new ObjectPool<RectTransform>(() => Instantiate(inventoryGrid).GetComponent<RectTransform>(), 
                obj =>
                {
                    obj.gameObject.SetActive(true); 
                    
                },
                obj =>
                {
                    obj.gameObject.SetActive(false); 
                    
                }, Destroy, false, 10000);
        }

        public void InventoryUpdate(int sizeX, int sizeY)
        {
            var sizeDelta = new Vector2(sizeX, sizeY) * ItemDatabaseManager.instance.itemSize;
            inventory.sizeDelta = sizeDelta;
            inventory.anchoredPosition = new Vector2(sizeDelta.x / 2, -sizeDelta.y / 2);

            if (inventoryGridObjects != null)
            {
                foreach (var obj in inventoryGridObjects)
                {
                    _gridObjectPool.Release(obj);
                }
            }
            inventoryGridObjects = new RectTransform[sizeX, sizeY];
            
            for (var i = 0; i < sizeX; i++)
            {
                for (var j = 0; j < sizeX; j++)
                {                  
                    inventoryGridObjects[j, i] = _gridObjectPool.Get();
                    inventoryGridObjects[j, i].SetParent(inventoryGridParent);
                }
            }
        }

        public void CreateBox(int sizeX, int sizeY)
        {
            itemBoxGridObjects = new RectTransform[sizeX, sizeY];
            itemBoxGridParent.sizeDelta = new Vector2(sizeX , sizeY)* ItemDatabaseManager.instance.itemSize;
            var a = 0;
            for (var i = 0; i < sizeY; i++)
            {
                for (var j = 0; j < sizeX; j++)
                {                  
                    itemBoxGridObjects[j, i] = _gridObjectPool.Get();
                    itemBoxGridObjects[j, i].SetParent(itemBoxGridParent);
                }
            }

            //InventoryManager.Instance.boxGrids = new bool[sizeX, sizeY];
            inGameUi[2].SetActive(true);
        }

        private void CloseBox()
        {
            if (itemBoxGridObjects == null)
            {
                return;
            }

            for (var index0 = 0; index0 < itemBoxGridObjects.GetLength(0); index0++)
            {
                for (var index1 = 0; index1 < itemBoxGridObjects.GetLength(1); index1++)
                {
                    var obj = itemBoxGridObjects[index0, index1];
                    obj.SetParent(null);
                    _gridObjectPool.Release(obj);
                }
            }

            inGameUi[2].SetActive(false);
        }

        public void InventoryOnOff(bool i)
        {
            inGameUi[0].SetActive(i);
            inGameUi[1].SetActive(i);
            inGameUi[3].SetActive(i);
            inInventoryItemParent.gameObject.SetActive(i);
            inBoxItemParent.gameObject.SetActive(i);
            inInventoryItemParent.gameObject.SetActive(i);
        }

        public void RootingMenuOn()
        {
            //Time.timeScale = i ? 0 : 1;
            boxName.text = InventoryManager.instance.openedBox.name;
            inGameUi[0].SetActive(true);
            InventoryManager.instance.OpenInventory();
            inGameUi[3].SetActive(true);
            inInventoryItemParent.gameObject.SetActive(true);
            inBoxItemParent.gameObject.SetActive(true);
        }

        public void RootingMenuOff()
        {
            inGameUi[0].SetActive(false);
            CloseBox();
            inGameUi[3].SetActive(false);
            inInventoryItemParent.gameObject.SetActive(false);
            inBoxItemParent.gameObject.SetActive(false);
        }
    }
}
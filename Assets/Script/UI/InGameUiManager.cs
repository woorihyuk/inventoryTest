using System;
using System.Collections.Generic;
using Script.Player;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Script.UI
{
    public class InGameUiManager : MonoBehaviour
    {
        // 이거 ui  출력 싸개로 변경하기!!!
        public GameObject[] inGameUi;
        /*
        background--0
        inventory--1
        rooting--2
        items--3
        */
        public RectTransform[,] inventoryGridObjects;
        public RectTransform[,] itemBoxGridObjects;
        
        public GameObject inventoryGrid;
        public RectTransform inventory;
        public RectTransform inventoryGridParent;
        public RectTransform itemBoxGridParent;
        public RectTransform itemBox;
        public List<GameObject> inInventoryItems; // 이거 인벤토리 메니저로 분리
        public List<GameObject> inBoxItems;
        
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

        public void OpenBox(int sizeX, int sizeY)
        {
            itemBoxGridObjects = new RectTransform[sizeX, sizeY];
            
            for (var i = 0; i < sizeX; i++)
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

        public void CloseBox()
        {
            if (itemBoxGridObjects == null)
            {
                return;
            }
            
            foreach (var obj in itemBoxGridObjects)
            {
                _gridObjectPool.Release(obj);
            }
        }

        public void InventoryOnOff(bool i)
        {
            inGameUi[0].SetActive(i);
            inGameUi[1].SetActive(i);
            inGameUi[3].SetActive(i);
        }

        public void RootingMenuOn(int sizeX, int sizeY)
        {
            //Time.timeScale = i ? 0 : 1;
            inGameUi[0].SetActive(true);
            InventoryManager.Instance.OpenInventory();
            inGameUi[3].SetActive(true);
        }

        public void RootingMenuOff()
        {
            inGameUi[0].SetActive(false);
            InventoryManager.Instance.OpenInventory();
            CloseBox();
            inGameUi[3].SetActive(false);
        }
    }
}
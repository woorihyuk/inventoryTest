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
        public RectTransform itemBox;
        public List<GameObject> inInventoryItems; // 이거 인벤토리 메니저로 분리
        public List<GameObject> inBoxItems;
        public int inventorySizeX;
        public int inventorySizeY;
        
        private IObjectPool<RectTransform> _inventoryGirdPool;
        
        private int _itemGridCount;

        private void Start()
        {
            // items = inGameUi[3].GetComponent<RectTransform>();
            // inventorySizeX = 5;
            // InventoryGrid = new ItemGrid[5, inventorySizeY];
            // itemBoxGrid = new ItemGrid[5, 8];
            
            _inventoryGirdPool = new ObjectPool<RectTransform>(() => Instantiate(inventoryGrid).GetComponent<RectTransform>(), 
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
                for (var i = 0; i < inventorySizeX; i++)
                {
                    for (var j = 0; j < inventorySizeY; j++)
                    {                  
                        _inventoryGirdPool.Release(inventoryGridObjects[i, j]);
                    }
                }
            }
            
            
            inventorySizeX = sizeX;
            inventorySizeY = sizeY;
            inventoryGridObjects = new RectTransform[sizeX, sizeY];
            
            for (var i = 0; i < inventorySizeX; i++)
            {
                for (var j = 0; j < inventorySizeY; j++)
                {                  
                    inventoryGridObjects[j, i] = _inventoryGirdPool.Get();
                    inventoryGridObjects[j, i].GetComponent<RectTransform>().SetParent(inventoryGridParent);
                }
            }
        }

        public void InventoryOnOff(bool i)
        {
            inGameUi[0].SetActive(i);
            inGameUi[1].SetActive(i);
            inGameUi[3].SetActive(i);
        }

        public void RootingMenuOn(bool i)
        {
            //Time.timeScale = i ? 0 : 1;
            inGameUi[0].SetActive(i);
            InventoryManager.Instance.OpenInventory();
            inGameUi[2].SetActive(i);
            inGameUi[3].SetActive(i);
        }
    }
}
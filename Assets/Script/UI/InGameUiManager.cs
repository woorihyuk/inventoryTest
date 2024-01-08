using System.Collections.Generic;
using UnityEngine;
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
        public List<ItemGrid> inventoryGridObjects;
        public ItemGrid[,] InventoryGrid;
        public RectTransform inventory;
        public RectTransform itemBox;
        public RectTransform items;

        public ItemGrid[] itemBoxGird;
        public ItemGrid[,] itemBoxGrid;

        public List<GameObject> inInventoryItems; // 이거 인벤토리 메니저로 분리
        public List<GameObject> inBoxItems;
        public int inventorySizeX;
        public int inventorySizeY;
        
        private int _itemGridCount;

        private void Awake()
        {
            items = inGameUi[3].GetComponent<RectTransform>();
            inventorySizeX = 5;
            InventoryGrid = new ItemGrid[5, inventorySizeY];
            itemBoxGrid = new ItemGrid[5, 8];
            for (var i = 0; i < inventorySizeY; i++)
            {
                for (var j = 0; j < inventorySizeX; j++)
                {                  
                    InventoryGrid[j, i] = inventoryGridObjects[_itemGridCount++];
                }
            }

            _itemGridCount = 0;
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    itemBoxGrid[j, i] = itemBoxGird[_itemGridCount++];
                }
            }
        }

        public void InventoryOn(bool i)
        {
            inGameUi[0].SetActive(i);
            inGameUi[1].SetActive(i);
            inGameUi[3].SetActive(i);
        }

        public void RootingMenuOn(bool i)
        {
            Time.timeScale = i ? 0 : 1;
            inGameUi[0].SetActive(i);
            inGameUi[1].SetActive(i);
            inGameUi[2].SetActive(i);
            inGameUi[3].SetActive(i);
        }
    }
}
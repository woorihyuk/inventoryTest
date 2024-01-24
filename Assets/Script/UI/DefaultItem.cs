using System;
using System.Collections.Generic;
using Script.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Script.UI
{
    public class DefaultItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //아이템 정보
         public InInventoryItemData itemData;

         public InGameUiManager InGameUiManager { private get; set; }
         public bool isRotation;

        private RectTransform _myTransform;
        private RectTransform _gridTransform;
        private Vector2 _dragStartPos;
        public bool isInInventory;
        private bool _isGrabbed;

        private void Awake()
        {
            _myTransform = GetComponent<RectTransform>();
            var childRectTransforms = GetComponentsInChildren<RectTransform>();
            _gridTransform = childRectTransforms[1];
        }

        public void RotateItem()
        {
            if (!_isGrabbed)
            {
                return;
            }
            isRotation = !isRotation;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isGrabbed = true;
            InventoryManager.instance.grabbedItem = this;
            var position = transform.position;
            _dragStartPos = position;
            OverLapp(isInInventory ? InventoryManager.instance.inventoryGrids : InventoryManager.instance.openedBox.boxGrids,
                itemData.posX, itemData.posY, false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isGrabbed = false;
            InventoryManager.instance.grabbedItem = null;
            var position = transform.position;
            var posX = -1;
            var posY = -1;
            var root = false;
            float gridDistance = 10000;
            var gridPos = Vector2.zero;
            RectTransform[,] grids;
            InInventoryItemData inventoryItemData;
            bool[,] gridCheck;
            // 인벤토리와 아이템 박스 중 어느 쪽에 가까운지 판단
            if (Vector2.Distance(position, InGameUiManager.inventory.position) > Vector2.Distance(position, InGameUiManager.itemBox.position))
            {
                // 아이템 박스
                //inventoryItemData = InventoryManager.instance.openedBox.inBoxItemData;
                gridCheck = InventoryManager.instance.openedBox.boxGrids;
                grids = InGameUiManager.itemBoxGridObjects;
                //lastItemList = InGameUiManager.inInventoryItems;
                //itemList = InGameUiManager.inBoxItems;
            }
            else
            {
                // 인벤토리
                //inventoryItemData = InventoryManager.instance.InInventoryItemData;
                gridCheck = InventoryManager.instance.inventoryGrids;
                grids = InGameUiManager.inventoryGridObjects;
                
                //lastItemList = InGameUiManager.inBoxItems;
                //itemList = InGameUiManager.inInventoryItems;
                root = true;
            }
            
            // 가장 가까운 그리드 찾기
            for (var i = 0; i < grids.GetLength(1); i++)
            {
                for (var j = 0; j < grids.GetLength(0); j++)
                {
                    var a = Vector2.Distance(grids[j, i].position,
                        _gridTransform.position);
                    if (a > gridDistance)
                    {
                        continue;
                    }
        
                    gridDistance = a;
                    gridPos = grids[j, i].position + transform.position -
                              _gridTransform.position;
                    posX = j;
                    posY = i;
                }
            }

            inventoryItemData = root ? InventoryManager.instance.GetItemData(posX, posY) : InventoryManager.instance.openedBox.GetItemData(posX, posY);
            
            if (gridDistance > 70 || InInventoryCheck(gridCheck, posX, posY))
            {
                OverLapp(gridCheck, itemData.posX, itemData.posY);
                transform.position = _dragStartPos;
                return; 
            }
            // 다른 아이템과 겹치는지 확인
            if (OverLapCheck(gridCheck, posX, posY))
            {
                print(gridCheck[posX, posY]);
                //같고 중첩 가능 아이템일 경우
                if (inventoryItemData.data.id == itemData.data.id && inventoryItemData.data.stackableItem)
                {
                    inventoryItemData.stack++;
                    ItemDatabaseManager.instance.RemoveItem(this);
                    return;
                }
            }

            //InStack();
            //grids[posX, posY].possessionItemData = itemName;
            OverLapp(gridCheck, posX, posY);
            itemData.posX = posX;
            itemData.posY = posY;

            transform.position = gridPos;
            if (root)
            {
                //InventoryManager.instance.inInventoryItemData[posX, posY] = itemData;
                InventoryManager.instance.AddToInventory(itemData);
            }
            else
            {
                _myTransform.SetParent(InGameUiManager.itemParent);
                InventoryManager.instance.openedBox.AddToBox(itemData);
                if (isInInventory)
                {
                    isInInventory = false;
                    InventoryManager.instance.RemoveFromInventory(itemData);
                }
            }
        }

        private void OverLapp(bool[,] itemGrids, int posX, int posY, bool isOverlap = true)
        {
            itemGrids[posX, posY] = isOverlap;
            for (var i = 0; i < (isRotation ? itemData.data.itemSizeX : itemData.data.itemSizeY); i++)
            {
                for (var j = 0; j < (isRotation ? itemData.data.itemSizeY : itemData.data.itemSizeX); j++)
                {
                    itemGrids[j + posX, i + posY] = isOverlap;
                }
            }
        }

        private bool InInventoryCheck(bool[,] itemGrids, int x, int y)
        {
            return x + (isRotation ? itemData.data.itemSizeY : itemData.data.itemSizeX) > itemGrids.GetLength(0) || y + (isRotation ? itemData.data.itemSizeX : itemData.data.itemSizeY) > itemGrids.GetLength(1);
        }

        private bool OverLapCheck(bool[,] itemGrids, int x, int y)
        {
            if (itemGrids[x, y]) return true;
            

            for (var k = 0; k < (isRotation ? itemData.data.itemSizeX : itemData.data.itemSizeY); k++)
            {
                for (var l = 0; l < (isRotation ? itemData.data.itemSizeY : itemData.data.itemSizeX); l++)
                {
                    if (!itemGrids[x + l, y + k]) continue;
                    return true;
                }
            }

            return false;
        }
        
        public void IntoItemBox(int posX = -1, int posY = -1)
        {
            var grid = InventoryManager.instance.openedBox.boxGrids;
            
            if (posX == -1 && posY == -1)
            {
                for (var i = 0; i < InGameUiManager.itemBoxGridObjects.GetLength(0); i++)
                {
                    for (var j = 0; j < InGameUiManager.itemBoxGridObjects.GetLength(1); j++)
                    {
                        if (OverLapCheck(grid, j, i)) continue;
                        OverLapp(grid, j, i);
                        _myTransform.anchoredPosition = InGameUiManager.itemBoxGridObjects[j, i].position +
                                                        (transform.position - _gridTransform.position);
                        itemData.posX = j;
                        itemData.posY = i;
                        return;
                    }
                }
            }
            
            _myTransform.anchoredPosition = InGameUiManager.itemBoxGridObjects[posX, posY].position + (transform.position - _gridTransform.position);
        }
    }
}
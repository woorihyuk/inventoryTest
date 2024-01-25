using System;
using System.Collections;
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
        [SerializeField]
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
            OverLapp(isInInventory ? InventoryManager.instance.inventoryOverlapInfo : InventoryManager.instance.openedBox.boxOverlapInfo,
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
            bool[,] overlapInfo;
            // 인벤토리와 아이템 박스 중 어느 쪽에 가까운지 판단
            var distanceForInventory = Vector2.Distance(position, InGameUiManager.inventory.position);
            if (Vector2.Distance(position, InGameUiManager.inventory.position) > Vector2.Distance(position, InGameUiManager.itemBox.position))
            {
                // 아이템 박스
                //inventoryItemData = InventoryManager.instance.openedBox.inBoxItemData;
                overlapInfo = InventoryManager.instance.openedBox.boxOverlapInfo;
                grids = InGameUiManager.itemBoxGridObjects;
                //lastItemList = InGameUiManager.inInventoryItems;
                //itemList = InGameUiManager.inBoxItems;
            }
            else
            {
                // 인벤토리
                //inventoryItemData = InventoryManager.instance.InInventoryItemData;
                overlapInfo = InventoryManager.instance.inventoryOverlapInfo;
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

            var inventoryItemData = root ? 
                InventoryManager.instance.GetItemData(posX, posY) 
                : InventoryManager.instance.openedBox.GetItemData(posX, posY);
            
            if (gridDistance > 70 || InInventoryCheck(overlapInfo, posX, posY))
            {
                GoBackItem(itemData.posX, itemData.posY);
                return; 
            }
            // 다른 아이템과 겹치는지 확인
            if (OverLapCheck(overlapInfo, posX, posY))
            {
                if (OverLapCheck(overlapInfo, posX, posY, true))
                {
                    print($"{posX}, {posY}");
                    print(overlapInfo[0, 0]);
                    if (inventoryItemData.data.id == itemData.data.id && inventoryItemData.data.stackableItem)
                    {
                        inventoryItemData.stack++;
                        ItemDatabaseManager.instance.RemoveItem(this);
                        return;
                    }
                    GoBackItem(itemData.posX, itemData.posY);
                    return; 
                }
                
                GoBackItem(itemData.posX, itemData.posY);
                return; 
            }

            //InStack();
            //grids[posX, posY].possessionItemData = itemName;
            OverLapp(overlapInfo, posX, posY);
            itemData.posX = posX;
            itemData.posY = posY;

            transform.position = gridPos;
            if (root)
            {
                isInInventory = true;
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

        private void GoBackItem(int x, int y)
        {
            OverLapp(isInInventory ? InventoryManager.instance.inventoryOverlapInfo : InventoryManager.instance.openedBox.boxOverlapInfo, x, y);
            transform.position = _dragStartPos;
        }

        private bool InInventoryCheck(bool[,] itemGrids, int x, int y)
        {
            if (x + (isRotation ? itemData.data.itemSizeY : itemData.data.itemSizeX) > itemGrids.GetLength(0) ||
                y + (isRotation ? itemData.data.itemSizeX : itemData.data.itemSizeY) > itemGrids.GetLength(1))
            {
                return true;
            }
            return false;
        }

        private bool OverLapCheck(bool[,] itemGrids, int x, int y, bool fit = false)
        {
            if (itemGrids[x, y]) return true;

            if (fit)
            {
                return false;
            }
            

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

        private IEnumerator ItemMoveDelay(int x, int y)
        {
            yield return null;
            _myTransform.position = InGameUiManager.itemBoxGridObjects[x, y].position + (transform.position - _gridTransform.position);
            //_myTransform.anchoredPosition = InGameUiManager.itemBoxGridObjects[x, y].position + (transform.position - _gridTransform.position);

            
        }
        
        public void IntoItemBox(int posX = -1, int posY = -1)
        {
            var grid = InventoryManager.instance.openedBox.boxOverlapInfo;
            
            if (posX == -1 && posY == -1)
            {
                for (var i = 0; i < InGameUiManager.itemBoxGridObjects.GetLength(0); i++)
                {
                    for (var j = 0; j < InGameUiManager.itemBoxGridObjects.GetLength(1); j++)
                    {
                        if (OverLapCheck(grid, j, i)) continue;
                        OverLapp(grid, j, i);
                        StartCoroutine(ItemMoveDelay(j, i));
                        _myTransform.anchoredPosition = InGameUiManager.itemBoxGridObjects[j, i].position +
                                                        (transform.position - _gridTransform.position);
                        itemData.posX = j;
                        itemData.posY = i;
                        return;
                    }
                }
            }
            
        }
    }
}
using System.Collections.Generic;
using Script.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Script.UI
{
    public class DefaultItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public ItemData thisData;
        //아이템 정보
        // public int id;
        // public string itemName;
        // public int itemPrise;
        // public string itemDescription;
        // public Vector2 itemSize;
        // public int countX;
        // public int countY;
        // public int stack;
        // public bool stackable;

        public InGameUiManager inGameUiManager;
        public GameObject gridPositionObj;
        public bool isRotation;

        private RectTransform _rectTransform;
        private RectTransform _gridPosition;
        private Vector2 _startPos;
        public bool isInInventory;
        private bool _isGrabbed;
        [FormerlySerializedAs("_gridPosX")] public int gridPosX;
        [FormerlySerializedAs("_gridPosY")] public int gridPosY;

        private void Awake()
        {
            inGameUiManager = FindFirstObjectByType<InGameUiManager>();
            _rectTransform = GetComponent<RectTransform>();
            var childRectTransforms = GetComponentsInChildren<RectTransform>();
            _gridPosition = childRectTransforms[1];
        }

        public void RotateItem()
        {
            if (!_isGrabbed)
            {
                return;
            }

            isRotation = !isRotation;
            //(countX, countY) = (countY, countX);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isGrabbed = true;
            InventoryManager.Instance.grabbedItem = this;
            var position = transform.position;
            _startPos = position;
            OverLapp(isInInventory ? InventoryManager.Instance.inventoryGrids : InventoryManager.Instance.openedBox.boxGrids,
                gridPosX, gridPosY, false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isGrabbed = false;
            InventoryManager.Instance.grabbedItem = null;
            var position = transform.position;
            var posX = -1;
            var posY = -1;
            var root = false;
            float gridDistance = 10000;
            var gridPos = Vector2.zero;
            RectTransform[,] grids;
            DefaultItem[,] posData;
            bool[,] gridCheck;
            List<GameObject> lastItemList;
            List<GameObject> itemList;
            // 인벤토리와 아이템 박스 중 어느 쪽에 가까운지 판단
            if (Vector2.Distance(position, inGameUiManager.inventory.position) > Vector2.Distance(position, inGameUiManager.itemBox.position))
            {
                // 아이템 박스
                posData = InventoryManager.Instance.openedBox.boxItemPosData;
                gridCheck = InventoryManager.Instance.openedBox.boxGrids;
                grids = inGameUiManager.itemBoxGridObjects;
                lastItemList = inGameUiManager.inInventoryItems;
                itemList = inGameUiManager.inBoxItems;
            }
            else
            {
                // 인벤토리
                posData = InventoryManager.Instance.inInventoryItems;
                gridCheck = InventoryManager.Instance.inventoryGrids;
                grids = inGameUiManager.inventoryGridObjects;
                
                lastItemList = inGameUiManager.inBoxItems;
                itemList = inGameUiManager.inInventoryItems;
                root = true;
            }
            
            // 가장 가까운 그리드 찾기
            for (var i = 0; i < grids.GetLength(1); i++)
            {
                for (var j = 0; j < grids.GetLength(0); j++)
                {
                    var a = Vector2.Distance(grids[j, i].position,
                        _gridPosition.position);
                    if (!(a < gridDistance)) continue;
        
                    gridDistance = a;
                    gridPos = grids[j, i].position + transform.position -
                              _gridPosition.position;
                    posX = j;
                    posY = i;
                }
            }
            Debug.Log($"{posX}, {posY}");
            
            // 그리드가 없는지 확인
            if (gridDistance > 70 || InInventoryCheck(gridCheck, posX, posY))
            {
                OverLapp(gridCheck, gridPosX, gridPosY);
                transform.position = _startPos;
                return; 
            }
            // 다른 아이템과 겹치는지 확인
            if (OverLapCheck(gridCheck, posX, posY))
            {
                print(gridCheck[posX, posY]);
                //같고 중첩 가능 아이템일 경우
                if (posData[posX, posY].thisData.itemId == thisData.itemId && posData[posX, posY].thisData.stackableItem)
                {
                    posData[posX, posY].thisData.stack++;
                    Items.instance.RemoveItem(this);
                }
            }
            isInInventory = root;
            //InStack();
            //grids[posX, posY].possessionItemData = itemName;
            OverLapp(gridCheck, posX, posY);
            InventoryManager.Instance.inInventoryItems[posX, posY] = this;
            gridPosX = posX;
            gridPosY = posY;

            transform.position = gridPos;
            
            lastItemList.Remove(gameObject); // 게임 매니저에서 아이템 리스트에서 제거
            itemList.Add(gameObject);// 게임 매니저에서 아이템 리스트에 추가
        }

        private void OverLapp(bool[,] itemGrids, int posX, int posY, bool isOverlap = true)
        {
            itemGrids[posX, posY] = isOverlap;
            for (var i = 0; i < (isRotation ? thisData.itemSizeX : thisData.itemSizeY); i++)
            {
                for (var j = 0; j < (isRotation ? thisData.itemSizeY : thisData.itemSizeX); j++)
                {
                    itemGrids[j + posX, i + posY] = isOverlap;
                }
            }
        }

        private bool InInventoryCheck(bool[,] itemGrids, int x, int y)
        {
            return x + (isRotation ? thisData.itemSizeY : thisData.itemSizeX) > itemGrids.GetLength(0) || y + (isRotation ? thisData.itemSizeX : thisData.itemSizeY) > itemGrids.GetLength(1);
        }

        private bool OverLapCheck(bool[,] itemGrids, int x, int y)
        {
            if (itemGrids[x, y]) return true;
            

            for (var k = 0; k < (isRotation ? thisData.itemSizeX : thisData.itemSizeY); k++)
            {
                for (var l = 0; l < (isRotation ? thisData.itemSizeY : thisData.itemSizeX); l++)
                {
                    if (!itemGrids[x + l, y + k]) continue;
                    print("here1");
                    return true;
                }
            }

            return false;
        }

        // private bool InInventoryCheck(Vector2 gridPos)
        // {
        //     var pos = gridPos + itemSize / 2;
        //     var position = inGameUiManager.inventory.position;
        //     if (inGameUiManager.inventory.sizeDelta.x / 2 < Vector2.Distance(new Vector2(pos.x, position.y), position) || inGameUiManager.inventory.sizeDelta.y / 2 < Vector2.Distance(new Vector2(position.x, pos.y), position))
        //     {
        //         return false;
        //     }
        //     
        //     if (inGameUiManager.inventory.sizeDelta.x / 2 <
        //         Vector2.Distance(new Vector2(pos.x, position.y), position) ||
        //         inGameUiManager.inventory.sizeDelta.y / 2 < Vector2.Distance(new Vector2(position.x, pos.y), position))
        //     {
        //     }
        //     
        //     return false;
        // }
        
        public void IntoItemBox()
        {
            var grid = InventoryManager.Instance.openedBox.boxGrids;
            for (var i = 0; i < inGameUiManager.itemBoxGridObjects.GetLength(0); i++)
            {
                for (var j = 0; j < inGameUiManager.itemBoxGridObjects.GetLength(1); j++)
                {
                    if (OverLapCheck(grid, j, i)) continue;
                    OverLapp(grid, j, i);
                    var position = transform.position;
                    _rectTransform.anchoredPosition = inGameUiManager.itemBoxGridObjects[j, i].position + (position - _gridPosition.position);
                    gridPosX = j;
                    gridPosY = i;
                    return;
                }
            }
        }
    }
}
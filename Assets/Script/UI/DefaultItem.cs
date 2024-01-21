using System.Collections.Generic;
using Script.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class DefaultItem : MonoBehaviour//, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //아이템 정보
        public string itemName;
        public int itemPrise;
        public string itemDescription;
        public Vector2 itemSize;
        public int countX;
        public int countY;
        public int stack;

        public GameObject gridPositionObj;
        
        private ItemGrid[,] _lastGrids;
        private InGameUiManager _inGameUiManager;
        private RectTransform _gridPosition;
        private Vector2 _startPos;
        private bool _isInInventory;
        private bool _isGrabbed;
        private int _gridPosX;
        private int _gridPosY;

        private void Awake()
        {
            _inGameUiManager = FindFirstObjectByType<InGameUiManager>();
            var childRectTransforms = GetComponentsInChildren<RectTransform>();
            _gridPosition = childRectTransforms[1];
        }

        public void RotateItem()
        {
            if (!_isGrabbed)
            {
                return;
            }
            (countX, countY) = (countY, countX);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isGrabbed = true;
            InventoryManager.Instance.grabbedItem = this;
            var position = transform.position;
            _startPos = position;
            if (_isInInventory)
            {
                OverLapp(InventoryManager.Instance.inventoryGrids, _gridPosX, _gridPosY, false);

            }
            else
            {
                for (var i = 0; i < countY; i++)
                {
                    for (var j = 0; j < countX; j++)
                    {
                        InventoryManager.Instance.inventoryGrids[j + _gridPosX, i + _gridPosY] = false;
                        //_inGameUiManager.itemBoxGrid[j + _gridPosX, i + _gridPosY].isOverlap = false;
                    }
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        // public void OnEndDrag(PointerEventData eventData)
        // {
        //     _isGrabbed = false;
        //     InventoryManager.Instance.grabbedItem = null;
        //     var position = transform.position;
        //     var posX = -1;
        //     var posY = -1;
        //     var root = false;
        //     float gridDistance = 10000;
        //     var gridPos = Vector2.zero;
        //     RectTransform[,] grids;
        //     bool[,] gridCheck;
        //     List<GameObject> lastItemList;
        //     List<GameObject> itemList;
        //     // 인벤토리와 아이템 박스 중 어느 쪽에 가까운지 판단
        //     if (Vector2.Distance(position, _inGameUiManager.inventory.position) > Vector2.Distance(position, _inGameUiManager.itemBox.position))
        //     {
        //         grids = _inGameUiManager.itemBoxGrid.GetComponent<RectTransform[,]>();
        //         lastItemList = _inGameUiManager.inInventoryItems;
        //         itemList = _inGameUiManager.inBoxItems;
        //     }
        //     else
        //     {
        //         gridCheck = InventoryManager.Instance.inventoryGrids;
        //         grids = _inGameUiManager.inventoryGridObjects.GetComponent<RectTransform>();
        //         
        //         lastItemList = _inGameUiManager.inBoxItems;
        //         itemList = _inGameUiManager.inInventoryItems;
        //         root = true;
        //     }
        //     
        //     // 가장 가까운 그리드 찾기
        //     for (var i = 0; i < grids.GetLength(1); i++)
        //     {
        //         for (var j = 0; j < grids.GetLength(0); j++)
        //         {
        //             var a = Vector2.Distance(grids[j, i].transform.position,
        //                 _gridPosition.position);
        //             if (!(a < gridDistance)) continue;
        //
        //             gridDistance = a;
        //             gridPos = grids[j, i].transform.position + transform.position -
        //                       _gridPosition.position;
        //             posX = j;
        //             posY = i;
        //         }
        //     }
        //     
        //     // 그리드가 없거나 겹치는지 확인
        //     if (gridDistance > 70 || InInventoryCheck(gridPos) || OverLapCheck(grids, posX, posY))
        //     {
        //         OverLapp(_lastGrids, _gridPosX, _gridPosY);
        //         transform.position = _startPos;
        //         return; 
        //     }
        //     _isInInventory = root;
        //     InStack();
        //     grids[posX, posY].possessionItemData = itemName;
        //     OverLapp(grids, posX, posY);
        //     _gridPosX = posX;
        //     _gridPosY = posY;
        //     _lastGrids = grids;
        //
        //     transform.position = gridPos;
        //     
        //     lastItemList.Remove(gameObject); // 게임 매니저에서 아이템 리스트에서 제거
        //     itemList.Add(gameObject);// 게임 매니저에서 아이템 리스트에 추가
        // }

        private void OverLapp(bool[,] itemGrids, int posX, int posY, bool isOverlap = true)
        {
            itemGrids[posX, posY] = isOverlap;
            for (var i = 0; i < countY; i++)
            {
                for (var j = 0; j < countX; j++)
                {
                    itemGrids[j + posX, i + posY] = isOverlap;
                }
            }
        }

        private bool OverLapCheck(ItemGrid[,] itemGrids, int x, int y)
        {
            if (itemGrids[x, y].isOverlap) return true;
            if (x + countX > itemGrids.GetLength(0) || y + countY > itemGrids.GetLength(1))
            {
                return true;
            }

            for (var k = 0; k < countY; k++)
            {
                for (var l = 0; l < countX; l++)
                {
                    if (!itemGrids[x + l, y + k].isOverlap) continue;
                    return true;
                }
            }

            return false;
        }

        private bool InInventoryCheck(Vector2 gridPos)
        {
            var pos = gridPos + itemSize / 2;
            var position = _inGameUiManager.inventory.position;
            if (_inGameUiManager.inventory.sizeDelta.x / 2 < Vector2.Distance(new Vector2(pos.x, position.y), position) || _inGameUiManager.inventory.sizeDelta.y / 2 < Vector2.Distance(new Vector2(position.x, pos.y), position))
            {
                return false;
            }
            
            if (_inGameUiManager.inventory.sizeDelta.x / 2 <
                Vector2.Distance(new Vector2(pos.x, position.y), position) ||
                _inGameUiManager.inventory.sizeDelta.y / 2 < Vector2.Distance(new Vector2(position.x, pos.y), position))
            {
            }
            
            return false;
        }
        
        // public void IntoItemBox()
        // {
        //     _lastGrids = _inGameUiManager.itemBoxGrid;
        //     var grid = _inGameUiManager.itemBoxGrid;
        //     for (var i = 0; i < 8; i++)
        //     {
        //         for (var j = 0; j < 5; j++)
        //         {
        //             if (OverLapCheck(grid, j, i)) continue;
        //             OverLapp(grid, j, i);
        //             var position = transform.position;
        //             transform.position = grid[j, i].transform.position + (position - _gridPosition.position);
        //             _gridPosX = j;
        //             _gridPosY = i;
        //             return;
        //         }
        //     }
        // }
    }
}
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
        [SerializeField] public InInventoryItemData itemData;

        public GridManager GridManager { private get; set; }
        public bool isRotation;

        private RectTransform _myTransform;
        private RectTransform _gridTransform;
        private RectTransform _horizontalGridTransform;
        private RectTransform _verticalGridTransform;
        private Vector2 _dragStartPos;
        private EquipmentType _curentEquipmentType;
        public bool isInInventory;
        private bool _isGrabbed;
        private bool _beforeRotate;

        private void Awake()
        {
            _myTransform = GetComponent<RectTransform>();
            var childRectTransforms = GetComponentsInChildren<RectTransform>();
            _horizontalGridTransform = childRectTransforms[1];
            _verticalGridTransform = childRectTransforms[2];
            _gridTransform = isRotation ? _verticalGridTransform : _horizontalGridTransform;
        }

        public void RotateItem()
        {
            if (!_isGrabbed)
            {
                return;
            }
            isRotation = !isRotation;
            _gridTransform = isRotation ? _verticalGridTransform : _horizontalGridTransform;
            transform.rotation = Quaternion.Euler(0, 0, isRotation ? -90 : 0);
        }

        public void SetParent(RectTransform parent)
        {
            _myTransform.SetParent(parent);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isGrabbed = true;
            InventoryManager.instance.GrabItem();
            var position = transform.position;
            _dragStartPos = position;
            _beforeRotate = isRotation;
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
            InventoryManager.instance.DropItem();
            var position = transform.position;
            var posX = -1;
            var posY = -1;
            var root = false;
            float gridDistance = 10000;
            var gridPos = Vector2.zero;
            RectTransform[,] grids = { };
            bool[,] overlapInfo = { };
            // 인벤토리와 아이템 박스 중 어느 쪽에 가까운지 판단
            var distanceForInventory = Vector2.Distance(position, GridManager.inventory.position);

            // 인벤토리와 가까움
            if (Vector2.Distance(position, GridManager.inventory.position) < gridDistance)
            {
                overlapInfo = InventoryManager.instance.inventoryOverlapInfo;
                grids = GridManager.inventoryGridObjects;
                root = true;
                gridDistance = Vector2.Distance(position, GridManager.inventory.position);
            }
            // 아이템 박스와 가까움
            if (Vector2.Distance(position, GridManager.itemBox.position)< gridDistance)
            {
                overlapInfo = InventoryManager.instance.openedBox.boxOverlapInfo;
                grids = GridManager.itemBoxGridObjects;
                gridDistance = Vector2.Distance(position, GridManager.itemBox.position);
            }

            var type = (EquipmentType)Math.Truncate((decimal)(itemData.data.id / 1000));

            switch (type)
            {
                case EquipmentType.Default:
                    break;
                case EquipmentType.Available:
                    break;
                case EquipmentType.Gun:
                    break;
                case EquipmentType.Weapon:
                    break;
                case EquipmentType.Armor:
                    break;
                case EquipmentType.HadGear:
                    break;
                case EquipmentType.Bag:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (Math.Abs(gridDistance - 10000) <= 0)
            {
                return;
            }

            
            // 가장 가까운 그리드 찾기
            var isOutGrid = true;
            
            for (var i = 0; i < grids.GetLength(1); i++)
            {
                for (var j = 0; j < grids.GetLength(0); j++)
                {
                    var dis = Vector2.Distance(grids[j, i].position, _gridTransform.position);
                    if (dis > gridDistance || dis > 70)
                    {
                        continue;
                    }

                    isOutGrid = false;
                    gridPos = grids[j, i].position + transform.position - _gridTransform.position;
                    posX = j;
                    posY = i;
                }
            }
            
            if (isOutGrid || InInventoryCheck(overlapInfo, posX, posY))
            {
                GoBack(itemData.posX, itemData.posY);
                return; 
            }
            
            // 다른 아이템과 겹치는지 확인
            if (OverLapCheck(overlapInfo, posX, posY))
            {
                if (!itemData.data.stackableItem)
                {
                    GoBack(itemData.posX, itemData.posY);
                    return; 
                }
                if (OverLapCheck(overlapInfo, posX, posY, true))
                {
                    var inventoryItemData = root ? 
                        InventoryManager.instance.GetItemData(posX, posY) 
                        : InventoryManager.instance.openedBox.GetItemData(posX, posY);
                    if (inventoryItemData.data.id == itemData.data.id)
                    {
                        inventoryItemData.stack++;
                        ItemDatabaseManager.instance.RemoveItem(this);
                        return;
                    }
                    GoBack(itemData.posX, itemData.posY);
                    return; 
                }
            }

            OverLapp(overlapInfo, posX, posY);
            itemData.posX = posX;
            itemData.posY = posY;
            
            transform.position = gridPos;
            if (root)
            {
                InventoryManager.instance.AddToInventory(itemData, _myTransform);
                if (!isInInventory)
                {
                    InventoryManager.instance.openedBox.RemoveFromBox(itemData);
                }
                isInInventory = true;
            }
            else
            {
                //_myTransform.SetParent(InGameUiManager.itemParent);
                InventoryManager.instance.openedBox.AddToBox(itemData, _myTransform);
                if (isInInventory)
                {
                    isInInventory = false;
                    InventoryManager.instance.RemoveFromInventory(itemData);
                }
            }
        }

        private void Drop()
        {
            
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

        private void GoBack(int x, int y)
        {
            isRotation = _beforeRotate;
            _gridTransform = _beforeRotate ? _verticalGridTransform : _horizontalGridTransform;
            Transform transform1;
            (transform1 = transform).rotation = Quaternion.Euler(0, 0, _beforeRotate ? -90 : 0);
            transform1.position = _dragStartPos;
            OverLapp(isInInventory ? InventoryManager.instance.inventoryOverlapInfo : InventoryManager.instance.openedBox.boxOverlapInfo, x, y);
        }

        private bool InInventoryCheck(bool[,] itemGrids, int x, int y)
        {
            return x + (isRotation ? itemData.data.itemSizeY : itemData.data.itemSizeX) > itemGrids.GetLength(0) ||
                   y + (isRotation ? itemData.data.itemSizeX : itemData.data.itemSizeY) > itemGrids.GetLength(1);
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
            _myTransform.position = GridManager.itemBoxGridObjects[x, y].position + (transform.position - _gridTransform.position);
        }

        private void IntoEquipmentSlot(EquipmentType type)
        {
            transform.position = this.GridManager.equipments[(int)type].position;
            isInInventory = true;
        }
        
        public void IntoItemBox(int posX = -1, int posY = -1)
        {
            var grid = InventoryManager.instance.openedBox.boxOverlapInfo;

            if (posX != -1 || posY != -1)
            {
                return;
            }
            for (var i = 0; i < GridManager.itemBoxGridObjects.GetLength(0); i++)
            {
                for (var j = 0; j < GridManager.itemBoxGridObjects.GetLength(1); j++)
                {
                    if (OverLapCheck(grid, j, i)) continue;
                    OverLapp(grid, j, i);
                    StartCoroutine(ItemMoveDelay(j, i));
                    _myTransform.anchoredPosition = GridManager.itemBoxGridObjects[j, i].position +
                                                    (transform.position - _gridTransform.position);
                    itemData.posX = j;
                    itemData.posY = i;
                    return;
                }
            }

        }
    }
}
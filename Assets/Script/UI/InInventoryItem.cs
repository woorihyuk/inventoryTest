using System;
using System.Collections.Generic;
using Script.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

namespace Script.UI
{
    public class InInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public float gridDistance;
        public float sizeX;
        public float sizeY;

        public RectTransform _gridPosition;
        private Vector2 _startPos;
        private Vector2 _pos;

        private void Start()
        {
        }

        private void Awake()
        {
            var position = transform.position;
            print(_gridPosition);
            var position1 = _gridPosition.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPos = transform.position;
        }


        public void OnDrag(PointerEventData eventData) 
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            gridDistance = 10000;
            Vector2 grid = default;
            for (int i = 0; i < InventoryManager.Instance.inventoryGrid.Length; i++)
            {
                var a = Vector2.Distance(InventoryManager.Instance.inventoryGrid[i].position,
                    (_gridPosition.position+(Vector3)_pos));
                if (a < gridDistance)
                {
                    gridDistance = a;
                    grid = InventoryManager.Instance.inventoryGrid[i].position;
                }
            }

            if (gridDistance>70)
            {
                transform.position = _startPos;
                return;
            }
            transform.position = grid+_pos;
        }
    }
}

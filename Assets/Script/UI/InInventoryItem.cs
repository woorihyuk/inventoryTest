using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class InInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public LayerMask itemContactLayerMask;
        public LayerMask inventoryBoxLayerMask;

        private readonly List<Collider2D> _colliders = new();
        private Collider2D _collider2D;
        private Vector3 _defaultPos;
        private ContactFilter2D _itemFilter;
        private ContactFilter2D _inventoryBoxFilter;

        private bool _isOverLap;
        private bool _isInInventory;

        public void Rooting()
        {
            Destroy(gameObject);
        }

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _itemFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = itemContactLayerMask
            };
            _inventoryBoxFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = inventoryBoxLayerMask
            };
        }

        private void Update()
        {
            var contactOtherItems = _collider2D.OverlapCollider(_itemFilter, _colliders);
            _isOverLap = contactOtherItems != 0;
            var contactInventory = _collider2D.OverlapCollider(_inventoryBoxFilter, _colliders);
            _isInInventory = contactInventory != 0;
        }


        
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _defaultPos = this.transform.position;
        }


        public void OnDrag(PointerEventData eventData)
        {
            transform.position += new Vector3(eventData.delta.x, eventData.delta.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isOverLap || !_isInInventory)
            {
                transform.position = _defaultPos;
            }
        }
    }
}

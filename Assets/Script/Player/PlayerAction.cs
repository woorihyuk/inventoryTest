using System;
using System.Collections.Generic;
using Script.Object;
using Script.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
    public class PlayerAction: MonoBehaviour
    {
        private InteractiveObjectChecker _interactiveObjectChecker;
        
        private InputAction _interaction;

        private void Awake()
        {
            _interaction = InputController.instance.inputActionAsset.FindAction("Interaction");
            _interaction.performed += _ => Interaction();
        }

        private void Start()
        {
            _interactiveObjectChecker = GetComponent<InteractiveObjectChecker>();
        }

        private void Update()
        {
        }

        public void Fire()
        {
            
        }

        public void Interaction()
        {
            if (!_interactiveObjectChecker.TryGetLastInteractiveObject(out var obj)) return;
            if (obj.objectType != InteractiveObjectType.ItemBox) return;
            var itemBox = obj.gameObject.GetComponent<ItemBox>();
            itemBox.OpenBox();
        }

        public void ItemRotation()
        {
            //InventoryManager.Instance.item
        }
    }
}
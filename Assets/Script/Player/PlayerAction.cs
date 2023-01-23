using System;
using System.Collections.Generic;
using Script.Object;
using Script.UI;
using UnityEngine;

namespace Script.Player
{
    public class PlayerAction: MonoBehaviour
    {
        private InteractiveObjectChecker _interactiveObjectChecker;
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
            if (_interactiveObjectChecker.TryGetLastInteractiveObject(out var obj))
            {
                if (obj.objectType==InteractiveObjectType.ItemBox)
                {
                    var itemBox = obj.gameObject.GetComponent<ItemBox>();
                    itemBox.OpenBox();
                }
            }
        }
    }
}
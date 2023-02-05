using System;
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
                if (obj.objectType==InteractiveObjectType.Item)
                {
                    var item = obj.gameObject.GetComponent<Item>();
                    item.Rooting();
                }
            }
        }
    }
}
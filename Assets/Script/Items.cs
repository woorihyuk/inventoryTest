using System;
using System.Collections.Generic;
using Script.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script
{
    public class Items : MonoBehaviour
    {
        public static Items Instance;
        public GameObject item;
        public ItemData[] data;
        public Transform inventory;
        private IObjectPool<GameObject> _itemPool;
        private InGameUiManager _inGameUiManager;

        private void Awake()
        {
            _inGameUiManager = FindFirstObjectByType<InGameUiManager>();
            Instance = this;
            _itemPool = new ObjectPool<GameObject>(() => Instantiate(item), 
                obj =>
                {
                    obj.SetActive(true); 
                    
                },
                obj =>
                {
                    obj.SetActive(false); 
                    
                }, Destroy, false, 10000);
        }

        public void AddItem(string itemSize, int itemType)
        {
            
        }
    }
}
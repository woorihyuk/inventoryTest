using System;
using System.Collections.Generic;
using Script.UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Script
{
    public class Items : MonoBehaviour
    {
        public static Items Instance;
        public IObjectPool<GameObject> ItemPool;
        public Canvas canvas;
        public GameObject item;
        public Transform _inItemBox;

        private Dictionary<string, int> _findItemSize;
        private Sprite[] _itemImage;

        private int[,] _itemSize;
        private int _cost;

        private void Awake()
        {
            Instance = this;
            ItemPool = new ObjectPool<GameObject>(() =>
                {
                    return Instantiate(item); 
                    
                }, obj =>
                {
                    obj.SetActive(true); 
                    
                },
                obj =>
                {
                    obj.SetActive(false); 
                    
                }, obj =>
                {
                    Destroy(obj);
                }, false, 10000);

            _itemSize = new int[3, 2] { { 80, 80 }, { 80, 150 }, { 150, 150 } };
            _findItemSize = new Dictionary<string, int>()
            {
                {"a", 0},
                {"b", 1}
            };
        }

        public void AddItem(string itemName)
        {
            GameObject iobj = Instance.ItemPool.Get();
            var rectTransform = iobj.GetComponent<RectTransform>();
            var rectTransformSizeDelta = rectTransform.sizeDelta;
            rectTransformSizeDelta.x = _itemSize[_findItemSize[itemName], 0];
            rectTransformSizeDelta.y = _itemSize[_findItemSize[itemName], 1];
            rectTransform.SetParent(_inItemBox);
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }
}
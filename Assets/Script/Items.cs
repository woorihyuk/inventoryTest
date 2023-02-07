using System;
using System.Collections.Generic;
using Script.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Script
{
    public class Items : MonoBehaviour
    {
        public static Items Instance;
        public IObjectPool<GameObject> ItemPool;
        public GameObject item;
        public Sprite[] itemSprite;
        /*
         0. 테스트용
         */
        public Transform inItemBox;

        private Dictionary<string, int> _findItemSize;
        private Sprite[] _itemImage;

        private float[,] _itemSize;
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

            _itemSize = new float[3, 2] { { 100, 100 }, { 100, 200 }, { 200, 200 } };
            _findItemSize = new Dictionary<string, int>()
            {
                {"1X1", 0},
                {"1X2", 1},
                {"2X2", 2}
            };
        }

        public void AddItem(string itemSize, int itemType)
        {
            GameObject iobj = Instance.ItemPool.Get();
            var rectTransform = iobj.GetComponent<RectTransform>();
            var image = iobj.GetComponent<Image>();
            var item = iobj.GetComponent<InInventoryItem>();
            rectTransform.sizeDelta = new Vector2(_itemSize[_findItemSize[itemSize], 0], _itemSize[_findItemSize[itemSize], 1]);
            item.sizeX = _itemSize[_findItemSize[itemSize], 0];
            item.sizeY = _itemSize[_findItemSize[itemSize], 1];
            image.sprite = itemSprite[itemType];
            rectTransform.SetParent(inItemBox);
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }
}
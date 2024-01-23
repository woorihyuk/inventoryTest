using System;
using Script.UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Script
{
    [Serializable]
    public class AllData
    {
        public ItemData[] ItemData;
    }
    public class Items : MonoBehaviour
    {
        //변수 이름 알잘딱하게 수정 필요
        public static Items instance;
        public GameObject item;
        public ItemData[] data;
        [SerializeField]
        public AllData itemDataAsset;
        public Transform inventory;
        private IObjectPool<DefaultItem> _itemPool;
        private InGameUiManager _inGameUiManager;
    
        public TextAsset itemData;
        [SerializeField] private int itemSize;
        
        //테스트용
        // private readonly ItemData _testData = new ItemData
        // {
        //     itemId = 1,
        //     itemName = "testItem",
        //     itemPrice = 1,
        //     stackableItem = false,
        //     itemSizeX = 1,
        //     itemSizeY = 2,
        //     itemDescription = null
        // };

        private void Awake()
        {
            _inGameUiManager = FindFirstObjectByType<InGameUiManager>();
            instance = this;
            _itemPool = new ObjectPool<DefaultItem>(() => Instantiate(item).GetComponent<DefaultItem>(), 
                obj =>
                {
                    obj.gameObject.SetActive(true); 
                    
                },
                obj =>
                {
                    obj.gameObject.SetActive(false); 
                    
                }, Destroy, false, 10000);
            
            itemDataAsset = JsonUtility.FromJson<AllData>(itemData.text);
            data = itemDataAsset.ItemData;
            print(data[0].itemName);
        }

        public DefaultItem RootItem(RectTransform itemParent, bool isInInventory)
        {
            var obj = _itemPool.Get();
            var rectTransform = obj.gameObject.GetComponent<RectTransform>();
            obj.data = data[4];
            obj.inGameUiManager = _inGameUiManager;
            obj.isInInventory = isInInventory;
            rectTransform.sizeDelta = new Vector2(itemSize*data[4].itemSizeX, itemSize*data[4].itemSizeY);
            rectTransform.SetParent(itemParent);
            obj.IntoItemBox();
            
            return obj;
        }

        public void RemoveItem(DefaultItem obj)
        {
            _itemPool.Release(obj);
        }
    }
}
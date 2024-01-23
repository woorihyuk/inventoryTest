using Script.UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Script
{
    public class Items : MonoBehaviour
    {
        public static Items instance;
        public GameObject item;
        public ItemData[] data;
        public Transform inventory;
        private IObjectPool<DefaultItem> _itemPool;
        private InGameUiManager _inGameUiManager;
        
        //테스트용
        private readonly ItemData _testData = new ItemData
        {
            itemId = 1,
            itemName = "testItem",
            itemPrice = 1,
            stack = 0,
            stackableItem = false,
            itemSizeX = 1,
            itemSizeY = 1,
            itemDescription = null
        };

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
        }

        public DefaultItem RootItem(RectTransform itemParent, bool isInInventory)
        {
            var obj = _itemPool.Get();
            var rectTransform = obj.gameObject.GetComponent<RectTransform>();
            obj.inGameUiManager = _inGameUiManager;
            obj.id = _testData.itemId;
            obj.itemPrise = _testData.itemPrice;
            obj.itemSize = new Vector2(_testData.itemSizeX, _testData.itemSizeY);
            obj.countX = _testData.itemSizeX;
            obj.countY = _testData.itemSizeY;
            obj.stackable = _testData.stackableItem;
            obj.isInInventory = isInInventory;
            
            rectTransform.sizeDelta = new Vector2(80, 160);
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
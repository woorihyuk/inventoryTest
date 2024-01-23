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

        [SerializeField] private int itemSize;
        
        //테스트용
        private readonly ItemData _testData = new ItemData
        {
            itemId = 1,
            itemName = "testItem",
            itemPrice = 1,
            stack = 0,
            stackableItem = false,
            itemSizeX = 1,
            itemSizeY = 2,
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
            obj.thisData = _testData;
            obj.inGameUiManager = _inGameUiManager;
            obj.isInInventory = isInInventory;
            rectTransform.sizeDelta = new Vector2(itemSize*_testData.itemSizeX, itemSize*_testData.itemSizeY);
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
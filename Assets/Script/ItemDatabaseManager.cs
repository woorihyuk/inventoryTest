using System;
using System.Collections.Generic;
using Script.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Script
{
    [Serializable]
    public class AllData
    {
        public ItemData[] itemData;
        
        
    }
    public class ItemDatabaseManager : MonoBehaviour
    {
        //변수 이름 알잘딱하게 수정 필요
        public static ItemDatabaseManager instance;
        
        // 아이템 프리펩
        public GameObject item;
        
        // id값으로 아이템 관리
        public Dictionary<int, ItemData> findItemData;
        
        // 아이템 데이터
        [SerializeField] public AllData itemDataAsset;
        public Transform inventory;
        
        // 아이템 오브젝트풀
        private IObjectPool<DefaultItem> _itemPool;
        
        private InGameUiManager _inGameUiManager;
    
        public TextAsset itemData;
        
        // 아이템 기준 크기
        public int itemSize;

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
            findItemData = new Dictionary<int, ItemData>();
            foreach (var data in itemDataAsset.itemData)
            {
                //print(data.id);
                findItemData.Add(data.id, data);
            }
        }

        public DefaultItem AddItem(int id)
        {
            var obj = _itemPool.Get();
            obj.itemData.data = findItemData[id];
            return obj;
        }

        public DefaultItem RootItem(int id, RectTransform itemParent)
        {
            var obj = _itemPool.Get();
            var rectTransform = obj.gameObject.GetComponent<RectTransform>();
            obj.itemData.data = findItemData[id];
            obj.InGameUiManager = _inGameUiManager;
            obj.isInInventory = false;
            rectTransform.sizeDelta = new Vector2(findItemData[id].itemSizeX, findItemData[id].itemSizeY)*itemSize;
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
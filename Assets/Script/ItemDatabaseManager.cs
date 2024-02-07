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
        private Dictionary<int, ItemData> _findItemData;
        
        // 아이템 데이터
        [SerializeField] public AllData itemDataAsset;
        public Transform inventory;
        
        // 아이템 오브젝트풀
        private IObjectPool<DefaultItem> _itemPool;
        
        private GridManager _gridManager;
    
        public TextAsset itemData;
        
        // 아이템 기준 크기
        public int itemSize;

        private void Awake()
        {
            _gridManager = FindFirstObjectByType<GridManager>();
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
            _findItemData = new Dictionary<int, ItemData>();
            foreach (var data in itemDataAsset.itemData)
            {
                //print(data.id);
                _findItemData.Add(data.id, data);
            }
        }

        public DefaultItem AddItem(int id, RectTransform itemParent)
        {
            var obj = _itemPool.Get();
            obj.itemData.data = _findItemData[id];
            obj.SetParent(itemParent);
            var type = (EquipmentType)Math.Truncate((decimal)(obj.itemData.data.id / 1000));

            switch (type)
            {
                case EquipmentType.Default:
                    break;
                case EquipmentType.Available:
                    break;
                case EquipmentType.Gun:
                    break;
                case EquipmentType.Weapon:
                    break;
                case EquipmentType.Armor:
                    break;
                case EquipmentType.HadGear:
                    break;
                case EquipmentType.Bag:
                    break;
            }
            
            return obj;
        }

        public DefaultItem RootItem(int id, RectTransform itemParent)
        {
            var obj = _itemPool.Get();
            var rectTransform = obj.gameObject.GetComponent<RectTransform>();
            obj.itemData.data = _findItemData[id];
            obj.GridManager = _gridManager;
            obj.isInInventory = false;
            rectTransform.sizeDelta = new Vector2(_findItemData[id].itemSizeX, _findItemData[id].itemSizeY)*itemSize;
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
using System.Collections.Generic;
using System.Linq;
using Script.Player;
using Script.UI;
using UnityEngine;

namespace Script.Object
{
    public class ItemBox : MonoBehaviour
    {
        //private readonly List<InInventoryItemData> _inBoxItems = new();
        public int boxSizeX;
        public int boxSizeY;
        private InInventoryItemData[,] _inBoxItemData;
        public bool[,] boxGrids;
        private InGameUiManager _inGameUiManager;
        
        private bool _isOpen;
        private bool _opened;

        private void Start()
        {
            _inGameUiManager = FindFirstObjectByType<InGameUiManager>();
            //_opened = true;
        }

        public void OpenBox()
        {
            if (_isOpen)
            {
                return;
            }
            InventoryManager.instance.openedBox = this;
            _inGameUiManager.CreateBox(boxSizeX, boxSizeY);
            if (!_opened)
            {
                _inBoxItemData = new InInventoryItemData[boxSizeX, boxSizeY];
                foreach (var val in _inBoxItemData)
                {
                    print(val.data.id);
                }
                boxGrids = new bool[boxSizeX, boxSizeY];
                var obj = ItemDatabaseManager.instance.RootItem(1001, _inGameUiManager.itemBox);
                _inBoxItemData[obj.itemData.posX, obj.itemData.posY] = obj.itemData;
                //_inBoxItems.Add(obj.itemData);
                _opened = true;
            }
            else
            {
                for (var index0 = 0; index0 < _inBoxItemData.GetLength(0); index0++)
                {
                    for (var index1 = 0; index1 < _inBoxItemData.GetLength(1); index1++)
                    {
                        if (_inBoxItemData[index0, index1].data == null)
                        {
                            continue;
                        }
                        var val = _inBoxItemData[index0, index1];
                        var obj = ItemDatabaseManager.instance.AddItem(val.data.id);
                        obj.IntoItemBox(val.posX, val.posY);
                    }
                }
                
            }
            _inGameUiManager.RootingMenuOn();
            _isOpen = true;
        }

        public void RemoveBox()
        {
            var items = _inGameUiManager.itemParent.GetComponentsInChildren<DefaultItem>();
            
            var removeObjList = new HashSet<DefaultItem>();
            for (var i = 0; i < items.Length; i++)
            {
                ItemDatabaseManager.instance.RemoveItem(items[i]);
            }

            // for (var i = 0; i < _inBoxItems.Count; i++)
            // {
            //     ItemDatabaseManager.instance.RemoveItem(_inBoxItems[i]);
            // }
        }
        
        public InInventoryItemData GetItemData(int x, int y)
        {
            return _inBoxItemData[x, y];
        }

        public void AddToBox(InInventoryItemData item)
        {
            _inBoxItemData[item.posX, item.posY] = item;
        }
        
        public void RemoveFromBox(InInventoryItemData data)
        {
            _inBoxItemData[data.posX, data.posY].data = null;
        }
    }
} 





    
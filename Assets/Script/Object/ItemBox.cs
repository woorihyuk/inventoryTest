using System.Collections.Generic;
using Script.Player;
using Script.UI;
using UnityEngine;

namespace Script.Object
{
    public class ItemBox : MonoBehaviour
    {
        public List<DefaultItem> inBoxItems;
        public int boxSizeX;
        public int boxSizeY;
        public DefaultItem[,] boxItemPosData;
        public bool[,] boxGrids;
        private InGameUiManager _inGameUiManager;
        private bool _opened;

        private void Start()
        {
            _inGameUiManager = FindFirstObjectByType<InGameUiManager>();
            //_opened = true;
        }

        public void OpenBox()
        {
            if (_opened)
            {
                return;
            }
            InventoryManager.Instance.openedBox = this;

            boxItemPosData = new DefaultItem[boxSizeX, boxSizeY];
            boxGrids = new bool[boxSizeX, boxSizeY];
            _inGameUiManager.OpenBox(boxSizeX, boxSizeY);
            _inGameUiManager.RootingMenuOn(boxSizeX, boxSizeY);
            inBoxItems.Add(Items.instance.RootItem(_inGameUiManager.itemBox, false));
            _opened = true;
        }
    }
} 





    
using System.Collections.Generic;
using Script.UI;
using UnityEngine;

namespace Script.Object
{
    public class ItemBox : MonoBehaviour
    {
        public List<DefaultItem> inBoxItems;
        public int boxSizeX;
        public int boxSizeY;
        private InGameUiManager _inGameUiManager;
        private bool _opened;

        private void Start()
        {
            _inGameUiManager = FindFirstObjectByType<InGameUiManager>();
            //_opened = true;
        }

        public void OpenBox()
        {
            print("open");
            _inGameUiManager.RootingMenuOn(true);
            //_opened = false;
        }
    }
} 





    
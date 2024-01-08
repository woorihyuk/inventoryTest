using System.Collections.Generic;
using Script.UI;
using UnityEngine;

namespace Script.Object
{
    public class ItemBox : MonoBehaviour
    {
        public List<DefaultItem> inBoxItems;
        private InGameUiManager _inGameUiManager;
        private bool _opened;

        private void Start()
        {
            _inGameUiManager = FindFirstObjectByType<InGameUiManager>();
            _opened = true;
        }

        public void OpenBox()
        {
            _inGameUiManager.RootingMenuOn(true);
            _opened = false;
        }
    }
} 





    
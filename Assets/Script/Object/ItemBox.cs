using System;
using Script.UI;
using UnityEngine;

namespace Script.Object
{
    public class ItemBox : MonoBehaviour
    {
        private bool opend;
        private class ItemInformation
        {
            public int ItemSizeNum;
        }

        private void Start()
        {
            opend = true;
        }

        public void OpenBox()
        {
            InGameUiManager.Instance.RootingMenuOn(true);
            print(Items.Instance);
            if(opend)Items.Instance.AddItem("2X2", 0);
            opend = false;
        }
    }
}
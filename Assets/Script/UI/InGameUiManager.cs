using System;
using UnityEngine;

namespace Script.UI
{
    public class InGameUiManager : MonoBehaviour
    {
        public static InGameUiManager Instance;

        public GameObject[] inGameUi;
        /*
        inventory--0
        rooting--1
        */

        private void Awake()
        {
            Instance = this;
        }

        public void InventoryOn(bool i)
        {
            Time.timeScale = i ? 0 : 1;
            inGameUi[0].SetActive(i);
        }

        public void RootingMenuOn(bool i)
        {
            Time.timeScale = i ? 0 : 1;
            inGameUi[0].SetActive(i);
            inGameUi[1].SetActive(i);
        }
    }
}
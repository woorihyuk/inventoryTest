using System;
using UnityEngine;

namespace Script.UI
{
    public class InGameUiManager : MonoBehaviour
    {
        public static InGameUiManager Instance;

        public GameObject[] inGameUi;
        /*
        background--0
        inventory--1
        rooting--2
        */

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < inGameUi.Length; i++)
            {
                inGameUi[i].SetActive(false);
            }
        }

        public void InventoryOn(bool i)
        {
            Time.timeScale = i ? 0 : 1;
            inGameUi[0].SetActive(i);
            inGameUi[1].SetActive(i);
        }

        public void RootingMenuOn(bool i)
        {
            Time.timeScale = i ? 0 : 1;
            inGameUi[0].SetActive(i);
            inGameUi[1].SetActive(i);
            inGameUi[2].SetActive(i);
        }
    }
}
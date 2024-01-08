using System;
using UnityEngine;

namespace Script.UI
{
    public class UiAction : MonoBehaviour
    {
        private InGameUiManager _inGameUiManager;

        private void Start()
        {
            _inGameUiManager = GetComponent<InGameUiManager>();
        }

        public void Escape()
        {
            foreach (var t in _inGameUiManager.inGameUi)
            {
                Time.timeScale = 1;
                t.SetActive(false);
            }
        }

        public void Rotate()
        {
            
        }
    }
}
using System;
using UnityEngine;

namespace Script.UI
{
    public class UiAction : MonoBehaviour
    {
        private GridManager _gridManager;

        private void Start()
        {
            _gridManager = GetComponent<GridManager>();
        }

        public void Escape()
        {
            foreach (var t in _gridManager.inGameUi)
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
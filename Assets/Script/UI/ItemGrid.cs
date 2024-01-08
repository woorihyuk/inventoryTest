using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Script.UI
{
    public class ItemGrid : MonoBehaviour
    {
        public string possessionItemData;
        public bool isOverlap;
        private Image _a;

        private void Awake()
        {
            _a = GetComponent<Image>();
        }

        private void Update()
        {
            _a.color = isOverlap ? Color.red : Color.green;
        }
    }
}
using System.Collections.Generic;
using Script.UI;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        public List<DefaultItem> inventoryContents;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            instance = this;
        }
        
        public void AddItem(DefaultItem defaultItem)
        {
            inventoryContents.Add(defaultItem);
        }
    }
}
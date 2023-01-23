using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Script
{
    public class Items : MonoBehaviour
    {
        public IObjectPool<GameObject> items;
        
        public GameObject item;

        private Sprite[] _itemImage;

        private int _cost, _sizeX, _sizeY;

        private void Awake()
        {
            // items=new ObjectPool<GameObject>(() =>
            // {
            //     return Instantiate(item);
            // }, obj =>
            // {
            //     obj.SetActive(true);
            // }, obj =>
            // {
            //     obj.SetActive(false);
            // })
        }

        // public GameObject AddItem()
        // {
        //     return 
        // }
    }
}
using Script.UI;
using UnityEngine;

namespace Script.Object
{
    public class ItemBox : MonoBehaviour
    {
        public void OpenBox()
        {
            InGameUiManager.Instance.RootingMenuOn(true);
        }
    }
}
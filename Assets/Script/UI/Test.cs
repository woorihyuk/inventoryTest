using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class Test : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
           print("dragg");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            print("drag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
    }
}
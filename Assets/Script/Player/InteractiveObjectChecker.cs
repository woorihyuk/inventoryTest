using System.Collections.Generic;
using UnityEngine;

namespace Script.Player
{
    public class InteractiveObjectChecker : MonoBehaviour
    {
        public LayerMask contactLayerMask;

        private readonly List<Collider2D> _colliders = new();

        private Collider2D _collider;
        private ContactFilter2D _filter;
        private InteractiveData _lastInteractiveObject;

        private class InteractiveData
        {
            public float Distance;
            public readonly InteractiveObject InteractiveObject;

            public InteractiveData(float dst, InteractiveObject iObj)
            {
                Distance = dst;
                InteractiveObject = iObj;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponentsInChildren<CircleCollider2D>()[0];
            _filter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = contactLayerMask
            };
        }

        // Update is called once per frame
        void Update()
        {
            var counts = _collider.OverlapCollider(_filter, _colliders);

            if (counts == 0)
            {
                _lastInteractiveObject?.InteractiveObject.OnDeselect();
                _lastInteractiveObject = null;
            }

            foreach (var col in _colliders)
            {
                var obj = col.GetComponent<InteractiveObject>();
                if (obj.objectType == InteractiveObjectType.None) continue;

                if (_lastInteractiveObject == null)
                {
                    _lastInteractiveObject =
                        new InteractiveData(Vector2.Distance(transform.position, obj.transform.position), obj);
                    obj.OnSelect();
                }
                else
                {
                    var dst = Vector2.Distance(transform.position, obj.transform.position);
                    if (_lastInteractiveObject.Distance < dst)
                    {
                        _lastInteractiveObject.Distance = dst;
                        continue;
                    }

                    if (_lastInteractiveObject.InteractiveObject != obj)
                    {
                        obj.OnSelect();
                        _lastInteractiveObject.InteractiveObject.OnDeselect();
                    }

                    _lastInteractiveObject = new InteractiveData(dst, obj);
                }
            }
        }

        public bool TryGetLastInteractiveObject(out InteractiveObject obj)
        {
            if (_lastInteractiveObject==null)
            {
                obj = null;
                return false;
                
            }
            obj = _lastInteractiveObject.InteractiveObject;
            return true;
        }
    }
}
using Script.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
    public class PlayerMove : MonoBehaviour
    {
        public LayerMask layerMask;

        public new BoxCollider2D collider2D;

        public float speed;

        private RaycastHit2D _hit;

        private Vector2 _distance, _rRayDistance, _lRayDistance;

        private bool _canMove;

        void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            var position = transform.position;
            var movePosition = _distance.normalized * (speed * Time.deltaTime);
            var bounds1 = collider2D.bounds;
            
           

            if (Physics2D.Raycast
                (position + new Vector3(bounds1.extents.x * _distance.x, bounds1.extents.y * _distance.y, 0),
                    _rRayDistance, collider2D.bounds.extents.x * 0.9f, layerMask)
                ||
                Physics2D.Raycast
                (position + new Vector3(bounds1.extents.x * _distance.x, bounds1.extents.y * _distance.y, 0),
                    _lRayDistance, collider2D.bounds.extents.x * 0.9f, layerMask)
               )
            {
                _canMove = false;
            }
            else
            {
                _canMove = true;
            }

            if (_canMove)
            {
                transform.position += (Vector3)movePosition;
            }
        }

        public void RayCast()
        {
            
        }

        public void OnPlayerMove(Vector2 direction)
        {
            _distance = direction;
            if (direction == new Vector2(0, 1))
            {
                _rRayDistance = new Vector2(1, 0);
                _lRayDistance = new Vector2(-1, 0);
            }
            else if (direction == new Vector2(1, 1))
            {
                _rRayDistance = new Vector2(0, -1);
                _lRayDistance = new Vector2(-1, 0);
            }
            else if (direction == new Vector2(1, 0))
            {
                _rRayDistance = new Vector2(0, -1);
                _lRayDistance = new Vector2(0, 1);
            }
            else if (direction == new Vector2(1, -1))
            {
                _rRayDistance = new Vector2(-1, 0);
                _lRayDistance = new Vector2(0, 1);
            }
            else if (direction == new Vector2(0, -1))
            {
                _rRayDistance = new Vector2(-1, 0);
                _lRayDistance = new Vector2(1, 0);
            }
            else if (direction == new Vector2(-1, -1))
            {
                _rRayDistance = new Vector2(0, 1);
                _lRayDistance = new Vector2(1, 0);
            }
            else if (direction == new Vector2(-1, 0))
            {
                _rRayDistance = new Vector2(0, 1);
                _lRayDistance = new Vector2(0, -1);
            }
            else if (direction == new Vector2(-1, 1))
            {
                _rRayDistance = new Vector2(1, 0);
                _lRayDistance = new Vector2(0, -1);
            }
        }
    }
}
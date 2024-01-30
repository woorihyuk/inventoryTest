using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private new BoxCollider2D collider2D;
        
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private float speed;

        private InputAction _move;
        
        private Vector2 _distance;
        
        private void Start()
        {
            _move = InputController.instance.inputActionAsset.FindActionMap("Player").FindAction("Move");
            _move.performed += OnPlayerMove;
            _move.canceled += _ =>
            {
                _distance = Vector2.zero;
            };
        }

        private void Update()
        {
            //print(InputController.instance.inputActionAsset.FindActionMap("Player").enabled);
            var position = transform.position;
            var bounds = collider2D.bounds;
            
            //상단 레이
            if (Physics2D.Raycast(position + new Vector3(bounds.size.x * 0.45f,bounds.extents.y), Vector2.left, bounds.size.x * 0.9f, layerMask))
            {
                _distance = new Vector2(_distance.x, _distance.y > 0 ? 0 : _distance.y);
            }
            //하단 레이
            if (Physics2D.Raycast(position + new Vector3(bounds.extents.x * 0.45f,-bounds.extents.y), Vector2.left, bounds.size.x * 0.9f, layerMask))
            {
                _distance = new Vector2(_distance.x, _distance.y < 0 ? 0 : _distance.y);
            }
            //좌측 레이
            if (Physics2D.Raycast(position + new Vector3(-bounds.extents.x,bounds.extents.y * 0.45f), Vector2.down, bounds.size.y * 0.9f, layerMask))
            {
                _distance = new Vector2(_distance.x < 0 ? 0 : _distance.x, _distance.y);
            }
            //우측 레이
            if (Physics2D.Raycast(position + new Vector3(bounds.extents.x,bounds.extents.y * 0.45f), Vector2.down, bounds.size.y * 0.9f, layerMask))
            {
                _distance = new Vector2(_distance.x > 0 ? 0 : _distance.x, _distance.y);
            }
            
            var movePosition = _distance * (speed * Time.deltaTime);
            transform.position += (Vector3)movePosition;
        }

        public void RayCast()
        {
            
        }

        private void OnPlayerMove(InputAction.CallbackContext context)
        {
            var dis = context.ReadValue<Vector2>();
            _distance = dis;
            // if (dis == new Vector2(0, 1))
            // {
            //     _rRayDistance = new Vector2(1, 0);
            //     _lRayDistance = new Vector2(-1, 0);
            // }
            // else if (dis == new Vector2(1, 1))
            // {
            //     _rRayDistance = new Vector2(0, -1);
            //     _lRayDistance = new Vector2(-1, 0);
            // }
            // else if (dis == new Vector2(1, 0))
            // {
            //     _rRayDistance = new Vector2(0, -1);
            //     _lRayDistance = new Vector2(0, 1);
            // }
            // else if (dis == new Vector2(1, -1))
            // {
            //     _rRayDistance = new Vector2(-1, 0);
            //     _lRayDistance = new Vector2(0, 1);
            // }
            // else if (dis == new Vector2(0, -1))
            // {
            //     _rRayDistance = new Vector2(-1, 0);
            //     _lRayDistance = new Vector2(1, 0);
            // }
            // else if (dis == new Vector2(-1, -1))
            // {
            //     _rRayDistance = new Vector2(0, 1);
            //     _lRayDistance = new Vector2(1, 0);
            // }
            // else if (dis == new Vector2(-1, 0))
            // {
            //     _rRayDistance = new Vector2(0, 1);
            //     _lRayDistance = new Vector2(0, -1);
            // }
            // else if (dis == new Vector2(-1, 1))
            // {
            //     _rRayDistance = new Vector2(1, 0);
            //     _lRayDistance = new Vector2(0, -1);
            // }
        }
    }
}
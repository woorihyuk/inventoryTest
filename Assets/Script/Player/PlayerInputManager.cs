using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
    public class PlayerInputManager : MonoBehaviour, NewInput.IPlayerActions
    {
        private PlayerMove _playerMove;
        private PlayerAction _playerAction;

        private NewInput _input;

        private void Awake()
        {
            _input = new NewInput();
            _input.Player.SetCallbacks(this);
            _input.Player.Enable();
            _playerMove = GetComponent<PlayerMove>();
            _playerAction = GetComponent<PlayerAction>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _playerMove.OnPlayerMove(context.ReadValue<Vector2>());
        }

        
        public void OnInteraction(InputAction.CallbackContext context)
        {
            _playerAction.Interaction();
        }
    }
}
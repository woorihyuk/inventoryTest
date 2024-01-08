using Script.Player;
using Script.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Script
{
    public class InputManager : MonoBehaviour, NewInput.IPlayerActions, NewInput.IUiInputActions
    {
        public PlayerMove playerMove;
        public PlayerAction playerAction;
        public UiAction uiAction;

        private NewInput _input;

        private void Awake()
        {
            _input = new NewInput();
            _input.Player.SetCallbacks(this);
            _input.UiInput.SetCallbacks(this);
            _input.Player.Enable();
            _input.UiInput.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            playerMove.OnPlayerMove(context.ReadValue<Vector2>());
        }

        
        public void OnInteraction(InputAction.CallbackContext context)
        {
            playerAction.Interaction();
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            uiAction.Escape();
        }

        public void OnItemRotate(InputAction.CallbackContext context)
        {
            //playerAction.
        }
    }
}
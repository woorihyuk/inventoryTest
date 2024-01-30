using Script.Player;
using Script.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Script
{
    public class InputController : MonoBehaviour
    {
        public static InputController instance;
        
        public InputActionAsset inputActionAsset;
        
        public PlayerMove playerMove;
        public PlayerAction playerAction;
        public UiAction uiAction;

        private NewInput _input;

        private void Awake()
        {
            instance = this;
            inputActionAsset.FindActionMap("Player").Enable();
        }

        
        public void OnInteraction(InputAction.CallbackContext context)
        {
            playerAction.Interaction();
        }

        public void OnOpenInventory(InputAction.CallbackContext context)
        {
            InventoryManager.instance.OpenInventory();
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
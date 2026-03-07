using System;
using UnityEngine;
using UnityEngine.InputSystem;
using YNQ.Dark.InventorySystem;
using YNQ.InteractionSystem;
using YNQ.Movement;

namespace YNQ.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private MovementController _movementController;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private InteractionController _interactionController;
        [SerializeField] private InventoryController _inventoryController;
        
        [Header("Movement")] 
        [SerializeField] private string _moveAction;
        [SerializeField] private string _runAction;
        [SerializeField] private string _rotateAction;
        [SerializeField] private string _jumpAction;
        [SerializeField] private string _crouchAction;
        
        [Header("Inventory")]
        [SerializeField] private string _hideItem;
        [SerializeField] private string _dropItem;
        [SerializeField] private string _item1;
        [SerializeField] private string _item2;
        [SerializeField] private string _item3;
        [SerializeField] private string _item4;
        
        [Header("Interaction")]
        [SerializeField] private string _interactionAction;
        
        private InputAction _moveActionRef;

        private void Start()
        {
            EnableInput();
            
            _moveActionRef = _playerInput.actions[_moveAction];
        }

        private void OnDestroy()
        {
            DisableInput();
        }

        private void EnableInput()
        {
            _playerInput.actions[_interactionAction].started += OnInteractionStart;
            _playerInput.actions[_interactionAction].canceled += OnInteractionEnd;
            
            _playerInput.actions[_runAction].started += OnRunStart;
            _playerInput.actions[_runAction].canceled += OnRunEnd;
            _playerInput.actions[_jumpAction].started += OnJump;
            _playerInput.actions[_crouchAction].started += ToggleCrouch;
            
            _playerInput.actions[_hideItem].started += HideItem;
            _playerInput.actions[_dropItem].started += DropItem;
            _playerInput.actions[_item1].started += Item1;
            _playerInput.actions[_item2].started += Item2;
            _playerInput.actions[_item3].started += Item3;
            _playerInput.actions[_item4].started += Item4;
            
            _playerInput.actions[_rotateAction].performed += OnRotate;
            
            _playerInput.actions.Enable();
        }

        private void DisableInput()
        {
            _playerInput.actions[_interactionAction].started -= OnInteractionStart;
            _playerInput.actions[_interactionAction].canceled -= OnInteractionEnd;
            
            _playerInput.actions[_runAction].started -= OnRunStart;
            _playerInput.actions[_runAction].canceled -= OnRunEnd;
            _playerInput.actions[_jumpAction].started -= OnJump;
            _playerInput.actions[_crouchAction].started -= ToggleCrouch;
            
            _playerInput.actions[_hideItem].started -= HideItem;
            _playerInput.actions[_dropItem].started -= DropItem;
            _playerInput.actions[_item1].started -= Item1;
            _playerInput.actions[_item2].started -= Item2;
            _playerInput.actions[_item3].started -= Item3;
            _playerInput.actions[_item4].started -= Item4;
            
            _playerInput.actions[_rotateAction].performed -= OnRotate;
            
            _playerInput.actions.Disable();
        }

        private void Update()
        {
            _movementController.SetInput(_moveActionRef.ReadValue<Vector2>());
        }

        #region MOVEMENT

        private void OnJump(InputAction.CallbackContext context)
        {
            _movementController.Jump();
        }

        private void ToggleCrouch(InputAction.CallbackContext context)
            => _movementController.SetCrouch(!_movementController.CrouchHeld);
        

        private void OnRunStart(InputAction.CallbackContext context)
            => _movementController.SetRun(true);
        
        private void OnRunEnd(InputAction.CallbackContext context)
            => _movementController.SetRun(false);

        private void OnRotate(InputAction.CallbackContext context)
        {
            var mouseDelta = context.ReadValue<Vector2>();
            
            _cameraController.Rotate(mouseDelta);
            _interactionController.MouseDelta = mouseDelta;
        }

        #endregion

        #region INVENTORY

        private void HideItem(InputAction.CallbackContext context) => _inventoryController.HideActiveItem();

        private void DropItem(InputAction.CallbackContext context) => _inventoryController.DropItemFromHand();

        private void Item1(InputAction.CallbackContext context) => _inventoryController.SwitchActiveItem(0);   
        private void Item2(InputAction.CallbackContext context) => _inventoryController.SwitchActiveItem(1);   
        private void Item3(InputAction.CallbackContext context) => _inventoryController.SwitchActiveItem(2);   
        private void Item4(InputAction.CallbackContext context) => _inventoryController.SwitchActiveItem(3);   
        
        #endregion

        private void OnInteractionStart(InputAction.CallbackContext context)
            => _interactionController.BeginInteraction();
        
        private void OnInteractionEnd(InputAction.CallbackContext context)
            => _interactionController.EndInteraction();
    }
}

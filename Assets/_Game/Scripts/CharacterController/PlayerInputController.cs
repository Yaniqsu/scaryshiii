using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
        
        [Header("Actions")] 
            [SerializeField] private string _interactionAction;
        [SerializeField] private string _moveAction;
        [SerializeField] private string _runAction;
        [SerializeField] private string _rotateAction;
        [SerializeField] private string _jumpAction;
        [SerializeField] private string _crouchAction;
        
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
            
            _playerInput.actions[_rotateAction].started += OnRotate;
        }

        private void DisableInput()
        {
            _playerInput.actions[_interactionAction].started -= OnInteractionStart;
            _playerInput.actions[_interactionAction].canceled -= OnInteractionEnd;
            
            _playerInput.actions[_runAction].started -= OnRunStart;
            _playerInput.actions[_runAction].canceled -= OnRunEnd;
            _playerInput.actions[_jumpAction].started -= OnJump;
            _playerInput.actions[_crouchAction].started -= ToggleCrouch;
            
            _playerInput.actions[_rotateAction].started -= OnRotate;
        }

        private void Update()
        {
            _movementController.SetInput(_moveActionRef.ReadValue<Vector2>());
        }

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

        private void OnInteractionStart(InputAction.CallbackContext context)
            => _interactionController.BeginInteraction();
        
        private void OnInteractionEnd(InputAction.CallbackContext context)
            => _interactionController.EndInteraction();
    }
}

using System;
using UnityEngine;
using UnityEngine.Events;

namespace YNQ.InteractionSystem
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private float _detectionlength;
        [SerializeField] private LayerMask _interactableLayer;

        public UnityEvent onPhysicsInteractableFound;
        public UnityEvent onPickableFound;
        public UnityEvent onInteractableLost;
        
        private IInteractable _currentInteractable;
        private Transform _currentInteractableTransform;
        private bool _inInteraction;
        private Camera _mainCamera;
        private Vector3 _grabPoint;
        private RaycastHit _hit;
        
        public Vector2 MouseDelta {get; set;}
        
        public UnityEvent onInteractionStart;
        public UnityEvent onInteractionEnd;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if(_inInteraction)
                UpdateInteractable();
            else
            {
                FindInteractable();
            }
        }

        private void FindInteractable()
        {
            if (Physics.SphereCast(_camera.position, 0.05f, _camera.forward, out _hit, _detectionlength,
                    _interactableLayer))
            {
                if(_hit.rigidbody.TryGetComponent(out IInteractable interactable))
                    GetInteractable(interactable, _hit.transform);
                else
                {
                    LostInteractable();
                }
            }
            else if(_currentInteractable != null)
                    LostInteractable();
        }

        private void UpdateInteractable()
        {
            if(Vector3.Distance(transform.position, _currentInteractableTransform.position) > _detectionlength)
            {
                EndInteraction();
                LostInteractable();
                return;
            }
            
            InteractionContext context = new InteractionContext
            {
                Camera = _mainCamera,
                Player = transform,
                Hit = _hit,
                MouseDelta = MouseDelta,
                GrabPointWorld = _grabPoint,
                DeltaTime = Time.deltaTime
            };

            _currentInteractable.InteractionUpdate(context);
        }

        private void GetInteractable(IInteractable interactable, Transform interactableTransform)
        {
            if (_currentInteractable == interactable)
                return;
            
            _currentInteractable = interactable;
            _currentInteractableTransform = interactableTransform;
            
            if(interactable is PhysicsInteractable)
                onPhysicsInteractableFound?.Invoke();
        }

        private void LostInteractable()
        {
            if (_currentInteractable == null)
                return;
            
            _currentInteractable = null;
            
            onInteractableLost?.Invoke();
        }

        public void BeginInteraction()
        {
            if (_currentInteractable == null)
                return;
            
            _inInteraction = true;
            _currentInteractable.BeginInteraction(transform);
            onInteractionStart?.Invoke();
        }

        public void EndInteraction()
        {
            if (_currentInteractable == null)
                return;
            
            _inInteraction = false;
            onInteractionEnd?.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.crimson;
            Gizmos.DrawRay(_camera.position, _camera.forward * _detectionlength);
        }
    }
}
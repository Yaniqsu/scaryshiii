using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YNQ.Dark.InventorySystem;

namespace YNQ.InteractionSystem
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private float _detectionlength;
        [SerializeField] private float _interactionlength;
        [SerializeField] private LayerMask _interactableLayer;

        public UnityEvent onPhysicsInteractableFound;
        public UnityEvent onPickableFound;
        public UnityEvent onInteractableLost;
        public UnityEvent<IInteractable> onInteractableFound;
        
        private IInteractable _currentInteractable;
        private Transform _currentInteractableTransform;
        private bool _inInteraction;
        private Camera _mainCamera;
        private Vector3 _grabPoint;
        private RaycastHit _hit;
        private HashSet<InteractionTag> _activeTags;
        
        public Vector2 MouseDelta {get; set;}
        public Vector2 MousePos { get; set; }
        
        public UnityEvent onInteractionStart;
        public UnityEvent onInteractionEnd;
        public UnityEvent<ItemInstance> onItemPickedUp;

        private void Start()
        {
            _mainCamera = Camera.main;
            
            _activeTags = new HashSet<InteractionTag>( new[]
            {
                InteractionTag.Default,
                InteractionTag.Psychics,
                InteractionTag.Pickable
            });
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
                if(_hit.rigidbody && 
                   _hit.rigidbody.TryGetComponent(out IInteractable interactable) &&
                   _activeTags.Contains(interactable.Tag))
                    GetInteractable(interactable, _hit.transform);
                else
                {
                    LostInteractable();
                }
            }
            else if (_currentInteractable != null)
            {
                LostInteractable();
            }
        }

        private void UpdateInteractable()
        {
            if (_currentInteractable.Type != InteractionType.Physics)
                return;
            
            if(Vector3.Distance(transform.position, _currentInteractableTransform.position) >
               (_inInteraction ? _interactionlength : _detectionlength))
            {
                EndInteraction();
                LostInteractable();
                return;
            }

            _currentInteractable.InteractionUpdate(CreateContext());
        }

        private void GetInteractable(IInteractable interactable, Transform interactableTransform)
        {
            if (_currentInteractable == interactable)
                return;

            _currentInteractable = interactable;
            _currentInteractableTransform = interactableTransform;
            
            onInteractableFound?.Invoke(interactable);
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
            _currentInteractable.BeginInteraction(CreateContext());
            onInteractionStart?.Invoke();

            if (_currentInteractable.Type == InteractionType.Short)
            {
                EndInteraction();
            }
        }

        public void EndInteraction()
        {
            if (_currentInteractable == null)
                return;
            
            _currentInteractable.EndInteraction();
            
            _inInteraction = false;
            onInteractionEnd?.Invoke();
        }

        public void AddInteractionTag(InteractionTag interactionTag)
        {
            _activeTags.Add(interactionTag);
        }

        public void RemoveInteractionTag(InteractionTag interactionTag)
        {
            _activeTags.Remove(interactionTag);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.crimson;
            Gizmos.DrawRay(_camera.position, _camera.forward * _detectionlength);
        }
        
        private InteractionContext CreateContext()
            => new InteractionContext
        {
            Camera = _mainCamera,
            Player = transform,
            Hit = _hit,
            MouseDelta = MouseDelta,
            MousePos = MousePos,
            GrabPointWorld = _grabPoint,
            DeltaTime = Time.deltaTime
        };
    }
}
using UnityEngine;

namespace YNQ.InteractionSystem
{
    public abstract class PhysicsInteractable : MonoBehaviour, IInteractable
    {
        public InteractionType Type => InteractionType.Physics;
        
        protected Rigidbody rb;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public abstract void BeginInteraction(Transform player);
        public abstract void InteractionUpdate(InteractionContext context);
        public abstract void EndInteraction();
    }
}
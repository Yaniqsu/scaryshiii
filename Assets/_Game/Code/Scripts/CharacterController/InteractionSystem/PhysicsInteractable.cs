using UnityEngine;

namespace YNQ.InteractionSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class PhysicsInteractable : MonoBehaviour, IInteractable
    {
        public InteractionType Type => InteractionType.Physics;
        public InteractionTag Tag => InteractionTag.Psychics;
        
        protected Rigidbody rb;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public abstract void BeginInteraction(InteractionContext context);
        public abstract void InteractionUpdate(InteractionContext context);
        public abstract void EndInteraction();
    }
}
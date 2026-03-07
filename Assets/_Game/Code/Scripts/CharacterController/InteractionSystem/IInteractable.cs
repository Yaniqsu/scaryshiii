using UnityEngine;

namespace YNQ.InteractionSystem
{
    public interface IInteractable
    {
        public InteractionType Type { get; }
        
        void BeginInteraction(Transform player);
        void InteractionUpdate(InteractionContext context);
        void EndInteraction();
    }
}
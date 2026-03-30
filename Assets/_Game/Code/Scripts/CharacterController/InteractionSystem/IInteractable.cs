using UnityEngine;

namespace YNQ.InteractionSystem
{
    public interface IInteractable
    {
        public InteractionType Type { get; }
        
        void BeginInteraction(InteractionContext context);
        void InteractionUpdate(InteractionContext context);
        void EndInteraction();
    }
}
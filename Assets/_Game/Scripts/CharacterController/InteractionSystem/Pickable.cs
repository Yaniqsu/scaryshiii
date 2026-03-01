using UnityEngine;

namespace YNQ.InteractionSystem
{
    public class Pickable : IInteractable
    {
        public InteractionType Type => InteractionType.Short;
        
        public void BeginInteraction(Transform player)
        {
            throw new System.NotImplementedException();
        }

        public void InteractionUpdate(InteractionContext context)
        {
            throw new System.NotImplementedException();
        }

        public void EndInteraction()
        {
            throw new System.NotImplementedException();
        }
    }
}
using UnityEngine;
using YNQ.InteractionSystem;

namespace YNQ.Dark.InventorySystem
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Pickable : MonoBehaviour, IInteractable
    {
        public InteractionType Type => InteractionType.Short;
        

        protected virtual void Awake() { }
        

        public abstract void BeginInteraction(InteractionContext context);

        public void InteractionUpdate(InteractionContext context) { }

        public void EndInteraction()
        {
            gameObject.SetActive(false);
        }
    }
}
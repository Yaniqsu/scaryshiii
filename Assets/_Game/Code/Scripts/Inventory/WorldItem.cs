using UnityEngine;
using YNQ.InteractionSystem;

namespace YNQ.Dark.InventorySystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class WorldItem : Pickable
    {
        [SerializeField] private ItemData data;
        
        private ItemInstance _instance;
        private Rigidbody _rb;
        private Collider _collider;

        protected override void Awake()
        {
            base.Awake();
            
            _instance = new ItemInstance(data);
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponentInChildren<Collider>();
        }

        public override void BeginInteraction(InteractionContext context)
        {
            if(context.Player.TryGetComponent(out InventoryController inventoryController))
            {
                inventoryController.AddItem(GetItemInstance());
            }
        }

        public ItemInstance GetItemInstance()
        {
            return _instance;
        }
        
        public void SetAsDynamic()
        {
            _rb.isKinematic = false;
            _collider.enabled = true;
        }

        public void SetAsInHand()
        {
            _rb.isKinematic = true;
            _collider.enabled = false;
        }
    }
}

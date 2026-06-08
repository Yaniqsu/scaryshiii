using UnityEngine;
using YNQ.InteractionSystem;

namespace YNQ.Dark.InventorySystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class WorldItem : Pickable
    {
        [Header("General")]
        [SerializeField] private ItemData data;
        
        private ItemInstance _instance;
        private ImpactSoundPlayer _impactSoundPlayer;
        private Rigidbody _rb;
        private Collider _collider;

        public ItemData ItemData => data;

        protected override void Awake()
        {
            base.Awake();
            
            _instance = new ItemInstance(data);
            _rb = GetComponent<Rigidbody>();
            _impactSoundPlayer = GetComponent<ImpactSoundPlayer>();
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
            _impactSoundPlayer.Enabled = true;
        }

        public void SetAsInHand()
        {
            _rb.isKinematic = true;
            _collider.enabled = false;
            _impactSoundPlayer.Enabled = false;
        }
    }
}

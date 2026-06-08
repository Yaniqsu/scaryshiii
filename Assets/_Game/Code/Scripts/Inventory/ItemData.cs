using FMODUnity;
using UnityEngine;

namespace YNQ.Dark.InventorySystem
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Objects /Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Identity")]
        [field: SerializeField] 
        public string ItemID { get; private set; }
        
        [field: SerializeField] 
        public string ItemName { get; private set; }
        
        [field: SerializeField] 
        public Sprite ItemSprite { get; private set; }
        
        
        [Header("Advanced Data")]
        [field: SerializeField] 
        public ItemType ItemType { get; private set; }
        
        [field: SerializeField] 
        public EMaterialType MaterialType { get; private set; }
        
        [field: SerializeField] 
        public ItemBehaviour Behaviour { get; private set; }
        
        
        [Header("World")]
        [field: SerializeField] 
        public WorldItem ItemPrefab { get; private set; }

        
        [Header("Audio")] 
        [field: SerializeField] 
        public EventReference PickUpSound { get; private set; }
    }
}

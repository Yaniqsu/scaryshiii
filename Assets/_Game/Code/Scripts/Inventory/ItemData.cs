using UnityEngine;

namespace YNQ.Dark.InventorySystem
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Objects /Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Identity")]
        public string itemID;
        public string itemName;
        public Sprite itemSprite;
        
        [Header("Advanced Data")]
        public ItemType itemType;
        public ItemBehaviour behaviour;
        
        [Header("World")]
        public WorldItem itemPrefab;
    }
}

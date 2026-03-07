using UnityEngine;

namespace YNQ.Dark.InventorySystem
{
    public abstract class ItemBehaviour : ScriptableObject
    {
        public abstract void Use(ItemInstance instance, GameObject user);
    }
}

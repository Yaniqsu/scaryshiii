using UnityEngine;

namespace YNQ.Dark.InventorySystem
{
    public abstract class ItemBehaviour : ScriptableObject
    {
        public abstract void Use(ItemInstance instance, GameObject user);
        public abstract void OnShow(ItemInstance instance, GameObject user);
        public abstract void OnHide(ItemInstance instance, GameObject user);
    }
}

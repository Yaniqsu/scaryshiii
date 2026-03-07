using UnityEngine;

namespace YNQ.Dark.InventorySystem
{
    public class ItemInstance
    {
        public ItemData ItemData { get; private set; }

        public ItemInstance(ItemData itemData)
        {
            ItemData = itemData;
        }

        public void Use(GameObject user)
        {
            ItemData.behaviour.Use(this, user);
        }
    }
}
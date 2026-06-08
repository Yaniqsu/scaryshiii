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
            ItemData.Behaviour.Use(this, user);
        }
        
        public void OnShow(GameObject user) => ItemData.Behaviour.OnShow(this, user);
        public void OnHide(GameObject user) => ItemData.Behaviour.OnHide(this, user);
    }
}
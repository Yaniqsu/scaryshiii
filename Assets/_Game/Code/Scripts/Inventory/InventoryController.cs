using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YNQ.Dark.InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Transform hand;
        [SerializeField] private int maxItems;

        private readonly List<ItemInstance> _backpack = new ();
        private ItemInstance _activeItem;
        private WorldItem _activeItemObject;
        private int _currentItemsCount;
        
        public bool ItemInHand { get; private set; }
        
        public UnityEvent<ItemInstance> onItemAdded;
        public UnityEvent<int> onItemRemoved;
        public UnityEvent onBackpackUpdated;
        public UnityEvent onInventoryOpen;
        public UnityEvent onInventoryClose;

        public void AddItem(ItemInstance instance)
        {
            if (ItemInHand && instance.ItemData.itemType == ItemType.Big)
            {
                if(instance.ItemData.itemType == ItemType.Big)
                    Debug.Log("Cannot hold 2 items in a hand");
                else
                {
                    AddItemToBackpack(instance);
                }
            }
            else
                AddItemToHand(instance);
        }

        public void UseActiveItem()
        {
            if (!ItemInHand)
                return;
            
            _activeItem.Use(gameObject);
        }

        private void AddItemToHand(ItemInstance data)
        {
            _activeItem = data;

            _activeItemObject = Instantiate(data.ItemData.itemPrefab, hand);
            _activeItemObject.SetAsInHand();
            
            _activeItemObject.transform.localPosition = Vector3.zero;
            _activeItemObject.transform.localRotation = Quaternion.identity;

            ItemInHand = true;
        }

        private void AddItemToBackpack(ItemInstance instance)
        {
            if (_currentItemsCount == maxItems)
                return;
            
            _backpack.Add(instance);
            _currentItemsCount++;
            
            onItemAdded?.Invoke(instance);
            onBackpackUpdated.Invoke();
        }

        private void RemoveItemFromBackpack(int index)
        {
            if (index >= _currentItemsCount)
                return;
            
            _backpack.RemoveAt(index);
            _currentItemsCount--;
            
            onItemRemoved?.Invoke(index);
            onBackpackUpdated.Invoke();
        }

        public void SwitchActiveItem(int index)
        {
            if (index < 0 || index >= _backpack.Count)
                return;

            if (ItemInHand)
            {
                Destroy(_activeItemObject.gameObject);
                
                var temp = _activeItem;
                AddItemToHand(_backpack[index]);
                _backpack[index] = temp;
            }
            else
            {
                AddItemToHand(_backpack[index]);
                RemoveItemFromBackpack(index);
            }
            
            onBackpackUpdated.Invoke();
        }

        public void HideActiveItem()
        {
            if (_activeItem == null || _currentItemsCount == maxItems)
                return;
            
            AddItemToBackpack(_activeItem);
            Destroy(_activeItemObject.gameObject);
            
            _activeItem = null;
            ItemInHand = false;
        }

        public void DropItemFromHand()
        {
            if (!ItemInHand)
                return;
            
            PlaceItemInTheWorld(_activeItemObject);
            
            _activeItem = null;
            ItemInHand = false;
        }

        public void DropItemFromBackpack(int index)
        {
            var item = _backpack[index];
            
            PlaceItemInTheWorld(Instantiate(item.ItemData.itemPrefab, hand));
            
            RemoveItemFromBackpack(index);
        }

        private static void PlaceItemInTheWorld(WorldItem item)
        {
            item.transform.SetParent(null);
            item.SetAsDynamic();
        }
    }
}
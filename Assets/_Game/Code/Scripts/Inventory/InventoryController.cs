using System;
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
        public ItemInstance ActiveItem { get; private set; }
        private WorldItem _activeItemObject;
        private HandController _handController;
        private int _currentItemsCount;
        
        public bool ItemInHand { get; private set; }
        
        public UnityEvent<ItemInstance> onItemAdded;
        public UnityEvent<int> onItemRemoved;
        public UnityEvent onBackpackUpdated;
        public UnityEvent onInventoryOpen;
        public UnityEvent onInventoryClose;

        private void Awake()
        {
            _handController = GetComponent<HandController>();
        }

        public void AddItem(ItemInstance instance)
        {
            if (ItemInHand && instance.ItemData.ItemType == ItemType.Big)
            {
                if(instance.ItemData.ItemType == ItemType.Big)
                    Debug.Log("Cannot hold 2 items in a hand");
                    //TODO: Add on screen message instead of the debug log                
                else
                {
                    AddItemToBackpack(instance);
                }
            }
            else
                AddItemToHand(instance);
            
            AudioManager.PlayOneShot(instance.ItemData.PickUpSound, transform.position);
        }

        public void UseActiveItem()
        {
            if (!ItemInHand)
                return;
            
            ActiveItem.Use(gameObject);
        }

        private void AddItemToHand(ItemInstance data)
        {
            ActiveItem = data;

            _activeItemObject = Instantiate(data.ItemData.ItemPrefab, hand);
            _activeItemObject.SetAsInHand();
            _handController.OccupyRightHand(_activeItemObject.gameObject);
            ActiveItem.OnShow(gameObject);

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
                
                var temp = ActiveItem;
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
            if (ActiveItem == null || _currentItemsCount == maxItems)
                return;
            
            AddItemToBackpack(ActiveItem);
            DestroyItemInHand();
        }

        public void DropItemFromHand()
        {
            if (!ItemInHand)
                return;

            _handController.FreeRightHand();
            PlaceItemInTheWorld(_activeItemObject);
            ActiveItem.OnHide(gameObject);
            
            ActiveItem = null;
            ItemInHand = false;
        }

        public void DropItemFromBackpack(int index)
        {
            var item = _backpack[index];
            
            PlaceItemInTheWorld(Instantiate(item.ItemData.ItemPrefab, hand));
            
            RemoveItemFromBackpack(index);
        }

        public void DestroyItemInHand()
        {
            _handController.FreeRightHand();
            ActiveItem.OnHide(gameObject);
            Destroy(_activeItemObject.gameObject);
            
            ActiveItem = null;
            ItemInHand = false;
        }

        private static void PlaceItemInTheWorld(WorldItem item)
        {
            item.transform.SetParent(null);
            item.SetAsDynamic();
        }
    }
}
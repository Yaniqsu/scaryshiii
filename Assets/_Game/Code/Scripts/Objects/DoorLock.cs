using UnityEngine;
using YNQ.Dark.InventorySystem;
using YNQ.InteractionSystem;

public class DoorLock : MonoBehaviour, IInteractable
{
    [SerializeField] private string _lockID;
    [SerializeField] private LockKey _key;
    
    public InteractionType Type => InteractionType.Short;
    public InteractionTag Tag => InteractionTag.DoorKey;
    
    public void BeginInteraction(InteractionContext context)
    {
        if (context.Player.TryGetComponent(out InventoryController inventoryController) &&
            inventoryController.ActiveItem.ItemData.itemID == _lockID)
        {
            inventoryController.DestroyItemInHand();
            _key.gameObject.SetActive(true);
            enabled = false;
        }
    }

    public void InteractionUpdate(InteractionContext context)
    {
    }

    public void EndInteraction()
    {
    }
}

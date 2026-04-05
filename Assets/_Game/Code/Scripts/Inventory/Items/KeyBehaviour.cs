using UnityEngine;
using YNQ.Dark.InventorySystem;
using YNQ.InteractionSystem;

[CreateAssetMenu(fileName = "Key Behaviour", menuName = "Scriptable Objects /Inventory /Key Behaviour")]
public class KeyBehaviour : ItemBehaviour
{
    public override void Use(ItemInstance instance, GameObject user) { }

    public override void OnShow(ItemInstance instance, GameObject user)
    {
        if(user.TryGetComponent(out InteractionController interactionController))
            interactionController.AddInteractionTag(InteractionTag.DoorKey);
    }

    public override void OnHide(ItemInstance instance, GameObject user)
    {
        if(user.TryGetComponent(out InteractionController interactionController))
            interactionController.RemoveInteractionTag(InteractionTag.DoorKey);
    }
}

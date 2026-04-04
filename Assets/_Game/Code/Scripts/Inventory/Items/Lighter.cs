using UnityEngine;
using YNQ.InteractionSystem;
using YNQ.Player;

public class Lighter : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Short;
    public InteractionTag Tag => InteractionTag.Pickable;
    
    public void BeginInteraction(InteractionContext context)
    {
        if (context.Player.TryGetComponent(out LighterController lighterController))
            lighterController.Enable();
    }

    public void InteractionUpdate(InteractionContext context) { }

    public void EndInteraction() { }
}

using UnityEngine;
using YNQ.InteractionSystem;

public class LockKey : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Physics;
    public InteractionTag Tag => InteractionTag.Default;
    
    public void BeginInteraction(InteractionContext context)
    {
        throw new System.NotImplementedException();
    }

    public void InteractionUpdate(InteractionContext context)
    {
        throw new System.NotImplementedException();
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}

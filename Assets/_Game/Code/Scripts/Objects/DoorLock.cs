using UnityEngine;
using YNQ.Dark.InventorySystem;
using YNQ.InteractionSystem;

public class DoorLock : MonoBehaviour, IInteractable
{
    [SerializeField] private string _lockID;
    [SerializeField] private LockKey _keyFront;
    [SerializeField] private LockKey _keyBack;
    [SerializeField] private Transform blockade;
    [SerializeField] private Vector3 blockadePosMin;
    [SerializeField] private Vector3 blockadePosMax;
    [SerializeField] private SoundBank soundBank;
    [SerializeField, SoundName(nameof(soundBank))] private string insertAudioSound;

    private Collider _collider;
    
    public InteractionType Type => InteractionType.Short;
    public InteractionTag Tag => InteractionTag.DoorKey;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void ToggleLock(bool open)
    {
        _keyBack.ForceRotation(open);
        _keyFront.ForceRotation(open);
        MoveBlockade(open ? 0 : 1);
    }

    public void BeginInteraction(InteractionContext context)
    {
        if (context.Player.TryGetComponent(out InventoryController inventoryController) &&
            inventoryController.ActiveItem.ItemData.ItemID == _lockID)
        {
            inventoryController.DestroyItemInHand();
            _collider.enabled = false;
            enabled = false;
            
            AudioManager.PlayOneShot(soundBank[insertAudioSound], transform.position);
            
            if(Vector3.Dot(transform.forward, context.Player.forward) > 0)
                _keyFront.gameObject.SetActive(true);
            else
                _keyBack.gameObject.SetActive(true);
        }
    }

    public void InteractionUpdate(InteractionContext context)
    {
    }

    public void EndInteraction()
    {
    }

    public void MoveBlockade(float t)
    {
        blockade.localPosition = Vector3.Lerp(blockadePosMin, blockadePosMax, t);
    }
}

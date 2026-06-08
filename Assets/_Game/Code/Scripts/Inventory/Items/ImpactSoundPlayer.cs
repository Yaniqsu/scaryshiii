using UnityEngine;
using YNQ.Dark.InventorySystem;

[RequireComponent(typeof(WorldItem))]
public class ImpactSoundPlayer : MonoBehaviour
{
    [SerializeField] private LayerMask impactLayer;
    [SerializeField] private ImpactSoundBank impactSoundBank;
    [SerializeField] private float velocityTreshold = 1;

    private EMaterialType _materialType;
    private Rigidbody _rigidbody;

    public bool Enabled { get; set; } = true;

    private void Awake()
    {
        _materialType = GetComponent<WorldItem>().ItemData.MaterialType;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Enabled && _rigidbody.linearVelocity.magnitude > velocityTreshold && CompareLayer(other.collider))
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (!TryGetSurfaceType(out var surfaceType)) return;
        
        var sound = impactSoundBank.GetSound(_materialType, surfaceType);

        if (sound.IsNull)
            return;
            
        AudioManager.PlayOneShot(sound, transform.position);
    }

    private bool TryGetSurfaceType(out ESurfaceType surfaceType)
    {
        surfaceType = ESurfaceType.Wood;
        var ray = Physics.Raycast(transform.position, Vector3.down, out var hitInfo, 1, impactLayer);

        if (ray && hitInfo.collider.gameObject.TryGetComponent(out SurfaceIdentifier surfaceIdentifier))
        {
            surfaceType = surfaceIdentifier.SurfaceType;
            return true;
        }
        
        return false;
    }

    private bool CompareLayer(Collider otherCollider)
    {
        return otherCollider & impactLayer != 0;
    }
}

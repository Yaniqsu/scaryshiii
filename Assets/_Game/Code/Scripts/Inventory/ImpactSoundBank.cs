using System;
using System.Linq;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "Impact Sound Bank", menuName = "Scriptable Objects /Impact Sound Bank")]
public class ImpactSoundBank : ScriptableObject
{
    [Serializable]
    private struct ImpactSound
    {
        [field: SerializeField] 
        public EMaterialType MaterialType { get; private set; }
        
        [field: SerializeField] 
        public ESurfaceType SurfaceType { get; private set; }
        
        [field: SerializeField] 
        public EventReference Sound { get; private set; }
    }

    [SerializeField] private ImpactSound[] sounds;

    public EventReference GetSound(EMaterialType materialType, ESurfaceType surfaceType)
        => (from sound in sounds where sound.SurfaceType == surfaceType && sound.MaterialType == materialType select sound.Sound)
            .FirstOrDefault();
}

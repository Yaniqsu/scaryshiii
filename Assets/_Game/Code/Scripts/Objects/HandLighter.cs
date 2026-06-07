using FMODUnity;
using UnityEngine;

public class HandLighter : MonoBehaviour
{
    private static readonly int LightTrigger = Animator.StringToHash("Light");
    
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private Animator animator;
    [SerializeField] private SoundBank lighterSoundBank;
    [SerializeField] private float lightVolumeIncrease = 0.2f;
    [SerializeField, SoundName(nameof(lighterSoundBank))] private string clickName;
    [SerializeField, SoundName(nameof(lighterSoundBank))] private string igniteName;
    [SerializeField, SoundName(nameof(lighterSoundBank))] private string extinguishName;

    private EventReference _ignite;
    private EventReference _extinguish;
    private EventReference _click;

    private void Awake()
    {
        _click = lighterSoundBank.GetEventReference(clickName);
        _ignite = lighterSoundBank.GetEventReference(igniteName);
        _extinguish = lighterSoundBank.GetEventReference(extinguishName);
    }

    public void TryLight(int attempts)
    {
        AudioManager.PlayOneShot(_click, transform.position, 
        ("lighter_click_gain", attempts * lightVolumeIncrease));
        animator.SetTrigger(LightTrigger);
    }
    
    public void Light()
    {
        fireParticles.Play();
        AudioManager.PlayOneShot(_ignite, transform.position);
    }

    public void Extinguish()
    {
        fireParticles.Stop();
        fireParticles.Clear();
        AudioManager.PlayOneShot(_extinguish, transform.position);
    }
}

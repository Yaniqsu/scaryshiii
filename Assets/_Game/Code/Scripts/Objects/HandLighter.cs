using UnityEngine;

public class HandLighter : MonoBehaviour
{
    private static readonly int LightTrigger = Animator.StringToHash("Light");
    
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private AudioSource lightSource;
    [SerializeField] private AudioSource extinguishSource;
    [SerializeField] private float baseLightVolume = 0.2f;
    [SerializeField] private float lightVolumeIncrease = 0.1f;

    public void TryLight(int attempts)
    {
        clickSource.Play();
        clickSource.volume = baseLightVolume + attempts * lightVolumeIncrease;
        
        animator.SetTrigger(LightTrigger);
    }
    
    public void Light()
    {
        fireParticles.Play();
        lightSource.Play();
    }

    public void Extinguish()
    {
        fireParticles.Stop();
        fireParticles.Clear();
        extinguishSource.Play();
    }
}

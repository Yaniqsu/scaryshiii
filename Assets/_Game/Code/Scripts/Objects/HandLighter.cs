using UnityEngine;

public class HandLighter : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private AudioSource lightSource;
    [SerializeField] private AudioSource extinguishSource;

    public void TryLight(int attempts)
    {
        clickSource.Play();
        clickSource.volume = 0.2f + attempts * 0.1f;
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

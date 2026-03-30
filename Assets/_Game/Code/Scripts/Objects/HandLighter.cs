using UnityEngine;

public class HandLighter : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private AudioSource lightSource;
    [SerializeField] private AudioSource extinguishSource;

    public void TryLight()
    {
        clickSource.Play();
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

using UnityEngine;
using YNQ.InteractionSystem;
using YNQ.Player;

public class Candles : MonoBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem[] fireParticles;
    [SerializeField] private new Light light;
    [SerializeField] private AudioSource igniteAudio;
    [SerializeField] private AudioSource extinguishAudio;
    [SerializeField] private AudioSource flameAudio;

    private bool _on;
    
    public InteractionType Type => InteractionType.Short;
    
    public void BeginInteraction(InteractionContext context)
    {
        if(_on)
            Extinguish();
        else if (context.Player.TryGetComponent(out LighterController controller)
            && controller.LighterOn)
        {
            Ignite();
        }
    }

    public void InteractionUpdate(InteractionContext context) { }

    public void EndInteraction() { }

    private void Ignite()
    {
        light.enabled = true;
        igniteAudio.Play();
        flameAudio.Play();
        _on = true;
        
        foreach (var fireParticle in fireParticles)
        {
            fireParticle.Play();
        }
    }
    
    private void Extinguish()
    {
        light.enabled = false;
        extinguishAudio.Play();
        flameAudio.Stop();
        _on = false;
        
        foreach (var fireParticle in fireParticles)
        {
            fireParticle.Stop();
            fireParticle.Clear();
        }
    }
}

using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using YNQ.InteractionSystem;
using YNQ.Player;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class Candles : MonoBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem[] fireParticles;
    [SerializeField] private new Light light;
    [SerializeField] private SoundBank soundBank;
    [SerializeField, SoundName(nameof(soundBank))] private string igniteAudio;
    [SerializeField, SoundName(nameof(soundBank))] private string extinguishAudio;
    [SerializeField, SoundName(nameof(soundBank))] private string flameAudio;

    [SerializeField] private bool _on;
    private EventInstance _flameInstance;

    public InteractionType Type => InteractionType.Short;
    public InteractionTag Tag { private set; get; } = InteractionTag.Candles;

    private void OnValidate()
    {
        light.enabled = _on;
    }

    private void Awake()
    {
        if (_on)
        {
            Tag = InteractionTag.Default;
            foreach (var fireParticle in fireParticles)
            {
                fireParticle.Play();
            }
        }
    }

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
        _on = true;
        Tag = InteractionTag.Default;
        
        AudioManager.PlayOneShot(soundBank[igniteAudio], transform.position);
        _flameInstance = AudioManager.instance.CreateInstance(soundBank[flameAudio]);
        _flameInstance.set3DAttributes(transform.position.To3DAttributes());
        _flameInstance.start();
        
        foreach (var fireParticle in fireParticles)
        {
            fireParticle.Play();
        }
    }
    
    private void Extinguish()
    {
        light.enabled = false;
        AudioManager.PlayOneShot(soundBank[extinguishAudio], transform.position);
        _flameInstance.stop(STOP_MODE.ALLOWFADEOUT);
        _on = false;
        Tag = InteractionTag.Candles;
        
        foreach (var fireParticle in fireParticles)
        {
            fireParticle.Stop();
            fireParticle.Clear();
        }
    }
}

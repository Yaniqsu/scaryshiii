using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> _eventInstances;
    private List<StudioEventEmitter> _eventEmitters;

    private EventInstance _ambienceEventInstance;
    private EventInstance _musicEventInstance;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        _eventInstances = new List<EventInstance>();
        _eventEmitters = new List<StudioEventEmitter>();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public static void PlayOneShot(EventReference sound, Vector3 worldPos,
        params (string paramName, float paramValue)[] parameters)
    {
        var eventInstance = FMODUnity.RuntimeManager.CreateInstance(sound);

        foreach (var param in parameters)
        {
            eventInstance.setParameterByName(param.paramName, param.paramValue);
        }

        eventInstance.set3DAttributes(worldPos.To3DAttributes());
        eventInstance.start();
        eventInstance.release();    
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        var eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        var emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        _eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        foreach (var eventInstance in _eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        
        foreach (var emitter in _eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
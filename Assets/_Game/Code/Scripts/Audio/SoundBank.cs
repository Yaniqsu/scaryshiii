using System;
using System.Linq;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Bank", menuName = "Scriptable Objects /Audio /Sound Bank")]
public class SoundBank : ScriptableObject
{
    [Serializable]
    public struct SoundReference
    {
        public string soundName;
        public EventReference eventReference;
    }
    
    [SerializeField] private SoundReference[] references;

    public EventReference GetEventReference(string eventName) => references.FirstOrDefault(r => r.soundName == eventName).eventReference;

    public string[] GetNames() => references.Select(r => r.soundName).ToArray();

    public EventReference this[string soundName] => GetEventReference(soundName);
}

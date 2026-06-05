using System;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Bank", menuName = "Scriptable Objects /Audio /Sound Bank")]
public class SoundBank : ScriptableObject
{
    private const string CLASS_TEMPLATE = 
        "";
    
    [Serializable]
    public struct SoundReference
    {
        public string soundName;
        public EventReference eventReference;
    }
    
    [SerializeField] private string bankName;
    [SerializeField] private SoundReference[] references;
    public static string filePath;
    
    public SoundReference[] References => references;
}

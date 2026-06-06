#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SoundName))]
public class SoundNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var soundName = (SoundName)attribute;
        
        if (string.IsNullOrWhiteSpace(soundName.SoundBankName))
        { 
            EditorGUI.HelpBox(position, "Sound bank name is empty", MessageType.Error);
            return;
        }
        
        var soundBankProperty = property.serializedObject.FindProperty(soundName.SoundBankName);
        
        if (soundBankProperty.objectReferenceValue == null)
        { 
            EditorGUI.HelpBox(position, $"{property.name} - Couldn't find sound bank: {soundName.SoundBankName}", MessageType.Error);
            return;
        }
        
        EditorGUI.BeginProperty(position, label, property);
        var soundBank = (SoundBank)soundBankProperty.objectReferenceValue;
        var soundNames = soundBank.GetNames();
        
        var currentIndex = System.Array.IndexOf(soundNames, property.stringValue);
        var newIndex = EditorGUI.Popup(position, label.text, currentIndex, soundNames);
        
        property.stringValue = 
            newIndex >= 0 && newIndex < soundNames.Length ? 
            soundNames[newIndex] : "(None)";
        
        EditorGUI.EndProperty();
    }
}

#endif

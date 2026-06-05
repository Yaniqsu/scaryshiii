using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundBank))]
public class SoundBankDrawer : Editor
{
    private SerializedProperty _bankNameProperty;
    private SerializedProperty _referencesProperty;

    public override void OnInspectorGUI()
    {
        _bankNameProperty = serializedObject.FindProperty("bankName");
        _referencesProperty = serializedObject.FindProperty("references");
        var bank = (SoundBank)serializedObject.targetObject;

        if (GUILayout.Button("Generate"))
            SoundBankGenerator.Generate(bank);
        EditorGUILayout.PropertyField(_bankNameProperty);
        EditorGUILayout.PropertyField(_referencesProperty);

        serializedObject.ApplyModifiedProperties();
    }
}

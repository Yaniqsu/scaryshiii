using UnityEngine;

public class SoundName : PropertyAttribute
{
    public readonly string SoundBankName;
    
    public SoundName(string soundBankName)
    {
        SoundBankName = soundBankName;
    }
}

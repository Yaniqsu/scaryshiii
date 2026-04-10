using UnityEngine;
using UnityEngine.Events;
using YNQ.InteractionSystem;

public class LockKey : ACircularMotionObject
{
    [SerializeField] private float unlockedValue;
    [SerializeField] private AudioSource turnAudio;
    
    public override InteractionTag Tag => InteractionTag.Default;

    public UnityEvent<float> onKeyMove;
    public UnityEvent onDoorLocked;
    public UnityEvent onDoorUnlocked;

    private bool _locked = true;

    public void ForceRotation(bool open)
    {
        rotation = new Vector3(0, 0, open ? rotationLimits.y : rotationLimits.x);
    }
    
    protected override void OnRotate()
    {
        var z = rotation.z;
        var min = rotationLimits.x;
        var max = rotationLimits.y;
        var t = Mathf.Abs(unlockedValue - Mathf.InverseLerp(min, max, z));
        
        onKeyMove.Invoke(t);

        switch (t)
        {
            case <= 0.1f when _locked:
                _locked = false;
                onDoorUnlocked.Invoke();
                break;
            case > 0.1f when !_locked:
                _locked = true;
                onDoorLocked.Invoke();
                break;
        }
    }

    protected override void OnRotationBegin()
    {
        turnAudio.Play();
    }

    protected override void OnRotationEnd()
    {
        turnAudio.Pause();
    }
}

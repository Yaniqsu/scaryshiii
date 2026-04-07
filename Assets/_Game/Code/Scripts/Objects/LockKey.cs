using UnityEngine;
using UnityEngine.Events;
using YNQ.InteractionSystem;

public class LockKey : ACircularMotionObject
{
    [SerializeField] private float unlockedValue;
    
    public override InteractionTag Tag => InteractionTag.Default;

    public UnityEvent<float> onKeyMove;
    public UnityEvent onDoorLocked;
    public UnityEvent onDoorUnlocked;

    private bool _locked = true;
    
    
    protected override void OnRotate(Vector3 rotation)
    {
        var z = rotation.z;
        var min = rotationLimits.x;
        var max = rotationLimits.y;
        var t = Mathf.Abs(unlockedValue - Mathf.InverseLerp(min, max, z));
        
        onKeyMove.Invoke(t);
        Debug.Log($"t: {t}");

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
}

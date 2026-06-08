using System;
using FMOD;
using FMOD.Studio;
using UnityEngine;
using YNQ.InteractionSystem;


[RequireComponent(typeof(HingeJoint))]
public class Door : PhysicsInteractable
{
    [Header("Main Settings")]
    [SerializeField] private bool locked;
    
    [Header("Interaction settings")]
    [SerializeField] private bool _checkDotProduct;
    [SerializeField] private float _angleTreshold = 0.1f;
    [SerializeField] private float lockTreshold = 0.3f;
    [SerializeField] private float blockadeAngle = 0.5f;
    [SerializeField] private float torqueStrength = 150f;
    [SerializeField] private float maxTorque = 300f;
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Components")] 
    [SerializeField] private DoorLock doorLock;

    [Header("Audio")] 
    [SerializeField] private SoundBank soundBank;
    [SerializeField, SoundName(nameof(soundBank))] private string openDoorSound;
    [SerializeField, SoundName(nameof(soundBank))] private string closeDoorSound;
    [SerializeField, SoundName(nameof(soundBank))] private string squeakSound;
    [SerializeField, SoundName(nameof(soundBank))] private string lockSound;
    [SerializeField, SoundName(nameof(soundBank))] private string unlockSound;

    private EventInstance _squeakInstance;

    private HingeJoint _hinge;

    private bool _isInteracting;
    private Vector3 _hingeAxisWorld;

    private float _smoothedInput;
    private float _inputVelocity;
    private int _torqueSign;
    private bool _doorClosed = true;
    
    protected override void Awake()
    {
        base.Awake();

        _hinge = GetComponent<HingeJoint>();
        
        _hingeAxisWorld = transform.TransformDirection(_hinge.axis);
        SetLockedLimits(locked);
    }

    private void Start()
    {
        _squeakInstance = AudioManager.instance.CreateInstance(soundBank[squeakSound]);
        
        doorLock.ToggleLock(!locked);
    }

    private void Update()
    {
        var speed = rb.angularVelocity.magnitude;
        _squeakInstance.setParameterByName("squeak_volume", Mathf.Clamp01(speed));
        _squeakInstance.getPlaybackState(out var state);
        
        if(speed > 0.05f)
        {
            if (state != PLAYBACK_STATE.PLAYING)
                _squeakInstance.start();
        }
        else
        {
            _squeakInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    public override void BeginInteraction(InteractionContext context)
    {
        _isInteracting = true;
        
        _torqueSign = _checkDotProduct ? 
            (int)Mathf.Sign(Vector3.Dot(context.Player.forward, rb.transform.forward)): -1;
    }

    public override void InteractionUpdate(InteractionContext context)
    {
        if (!_isInteracting) return;
        
        float rawInput = context.MouseDelta.y * mouseSensitivity;

        _smoothedInput = Mathf.SmoothDamp(
            _smoothedInput,
            rawInput,
            ref _inputVelocity,
            0.05f
        );

        float torqueAmount = Mathf.Clamp(
            _smoothedInput * torqueStrength,
            -maxTorque,
            maxTorque
        ) * _torqueSign;
        
        rb.AddTorque(_hingeAxisWorld * torqueAmount, ForceMode.Force);
        
        if(_hinge.angle > _angleTreshold)
            OpenDoor();
        else if(float.IsNaN(_hinge.angle) || _hinge.angle < _angleTreshold)
            CloseDoor();
    }

    public override void EndInteraction()
    {
        _isInteracting = false;
    }

    private void CloseDoor()
    {
        if (_doorClosed)
            return;
        
        _doorClosed = true;
        
        var speed = rb.angularVelocity.magnitude;
        AudioManager.PlayOneShot(soundBank[closeDoorSound], transform.position,
            ("door_close_gain", Mathf.Clamp01(speed)));
    }

    private void OpenDoor()
    {
        if (!_doorClosed)
            return;
        
        _doorClosed = false;
        AudioManager.PlayOneShot(soundBank[openDoorSound], transform.position);
    }

    public void ToggleLocked(bool locked)
    {
        if (float.IsNaN(_hinge.angle) || _hinge.angle <= lockTreshold)
        {
            this.locked = locked;

            AudioManager.PlayOneShot(this.locked ? soundBank[lockSound] : soundBank[unlockSound], transform.position);
        }
        
        SetLockedLimits(locked);
    }

    private void SetLockedLimits(bool locked)
    {
        var limits = _hinge.limits;
        limits.max = this.locked ? lockTreshold : 120;
        limits.min = locked && !float.IsNaN(_hinge.angle) && _hinge.angle > lockTreshold ? blockadeAngle : 0;
        _hinge.limits = limits;
    }
}

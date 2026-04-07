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
    [SerializeField] private AudioSource _openDoorSource;
    [SerializeField] private AudioSource _closeDoorSource;
    [SerializeField] private AudioSource _squeakSource;

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
        ToggleLocked(locked);
    }
    
    private void Update()
    {
        var speed = rb.angularVelocity.magnitude;
        _squeakSource.volume = Mathf.Clamp01(speed);

        if (speed > 0.05f)
        {
            if (!_squeakSource.isPlaying)
                _squeakSource.Play();
        }
        else
        {
            _squeakSource.Stop();
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
        _closeDoorSource.volume = Mathf.Clamp01(speed);
        _closeDoorSource.Play();
    }

    private void OpenDoor()
    {
        if (!_doorClosed)
            return;
        
        _doorClosed = false;
        _openDoorSource.Play();
    }

    public void ToggleLocked(bool locked)
    {
        if (float.IsNaN(_hinge.angle) || _hinge.angle <= lockTreshold)
        {
            this.locked = locked;
        }
        
        var limits = _hinge.limits;
        limits.max = this.locked ? lockTreshold : 120;
        limits.min = locked && !float.IsNaN(_hinge.angle) && _hinge.angle > lockTreshold ? blockadeAngle : 0;
        _hinge.limits = limits;
    }
}

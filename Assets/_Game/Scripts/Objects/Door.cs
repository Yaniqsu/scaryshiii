using UnityEngine;
using YNQ.InteractionSystem;

public class Door : PhysicsInteractable
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private HingeJoint _hinge;
    [SerializeField] private AudioSource _openDoorSource;
    [SerializeField] private AudioSource _closeDoorSource;
    [SerializeField] private AudioSource _squeakSource;
    [SerializeField] private float torqueStrength = 150f;
    [SerializeField] private float maxTorque = 300f;
    [SerializeField] private float mouseSensitivity = 2f;


    private bool _isInteracting;
    private Vector3 _hingeAxisWorld;

    private float _smoothedInput;
    private float _inputVelocity;
    private int _torqueSign;
    private bool _doorClosed = true;
    
    protected override void Awake()
    {
        base.Awake();
        
        _hingeAxisWorld = transform.TransformDirection(_hinge.axis);
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

    public override void BeginInteraction(Transform player)
    {
        _isInteracting = true;
        
        _torqueSign = (int)Mathf.Sign(Vector3.Dot(player.forward, _rb.transform.forward));
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
        
        _rb.AddTorque(_hingeAxisWorld * torqueAmount, ForceMode.Force);
        
        if(_hinge.angle > 0.1f && _doorClosed)
            OpenDoor();
        else if(_hinge.angle < 0.1f && !_doorClosed)
            CloseDoor();
    }

    public override void EndInteraction()
    {
        _isInteracting = false;
    }

    private void CloseDoor()
    {
        _doorClosed = true;
        
        var speed = rb.angularVelocity.magnitude;
        _closeDoorSource.volume = Mathf.Clamp01(speed);
        _closeDoorSource.Play();
    }

    private void OpenDoor()
    {
        _doorClosed = false;
        _openDoorSource.Play();
    }
}

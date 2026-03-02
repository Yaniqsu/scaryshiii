using UnityEngine;
using YNQ.InteractionSystem;

public class Drawer : PhysicsInteractable
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ConfigurableJoint _joint;
    [SerializeField] private AudioSource _closeSource;
    [SerializeField] private AudioSource _moveSource;
    [SerializeField] private float _openTreshold;
    [SerializeField] private float _closeTreshold;
    [SerializeField] private float moveSpeed = 150f;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float mouseSensitivity = 2f;

    private bool _isInteracting;
    private Vector3 _moveDirection;

    private float _smoothedInput;
    private float _inputVelocity;
    private int _moveSign;
    private bool _doorClosed = true;
    private Vector3 _startPos;
    
    protected override void Awake()
    {
        base.Awake();
        
        _moveDirection = transform.TransformDirection(_joint.axis);
        _startPos = transform.localPosition;
    }
    
    private void Update()
    {
        var speed = rb.linearVelocity.magnitude;
        _moveSource.volume = Mathf.Clamp01(speed);

        if (speed > 0.01f)
        {
            if (!_moveSource.isPlaying)
                _moveSource.Play();
        }
        else
        {
            _moveSource.Stop();
        }
    }

    public override void BeginInteraction(Transform player)
    {
        _isInteracting = true;
        
        _moveSign = (int)Mathf.Sign(Vector3.Dot(player.forward, -rb.transform.forward));

        _rb.isKinematic = false;
    }

    public override void InteractionUpdate(InteractionContext context)
    {
        if (!_isInteracting) return;
        
        float rawInput = -context.MouseDelta.y * mouseSensitivity;

        _smoothedInput = Mathf.SmoothDamp(
            _smoothedInput,
            rawInput,
            ref _inputVelocity,
            0.05f
        );

        var moveAmount = Mathf.Clamp(
            _smoothedInput * moveSpeed,
            -maxSpeed,
            maxSpeed
        );
        
        _rb.AddForce(_moveDirection * moveAmount, ForceMode.Force);
        
        var distance = Vector3.Distance(transform.localPosition, _startPos);
        
        if(distance > _openTreshold && _doorClosed)
            OpenDoor();
        else if(distance < _closeTreshold && !_doorClosed)
            CloseDoor();
    }

    public override void EndInteraction()
    {
        _isInteracting = false;
        _rb.isKinematic = true;
    }

    private void CloseDoor()
    {
        _doorClosed = true;

        _closeSource.volume = Mathf.Clamp01(_rb.linearVelocity.magnitude);
        _closeSource.Play();
    }

    private void OpenDoor()
    {
        _doorClosed = false;
    }
}

using UnityEngine;
using UnityEngine.Events;
using YNQ.Movement.States;

namespace YNQ.Movement
{
    public class MovementController : MonoBehaviour
    {
        [Header("Speeds")]
        public float walkSpeed = 4f;
        public float runSpeed = 7f;
        public float crouchSpeed = 2f;

        [Header("Jump")]
        public float jumpForce = 1.6f;
        public float gravityMultiplier = 2f;

        [Header("Acceleration")]
        public float acceleration = 20f;
        public float deceleration = 25f;
        
        public UnityEvent<AMovementState> OnStateChanged;

        public CharacterController Character { get; private set; }

        public Vector2 Input { get; private set; }
        public bool RunHeld { get; private set; }
        public bool CrouchHeld { get; private set; }
        public bool JumpPressed { get; private set; }

        public Vector3 HorizontalVelocity { get; private set; }
        public float VerticalVelocity { get; set; }

        public AMovementState CurrentState { get; private set; }

        public StandingState Standing { get; private set; }
        public WalkingState Walking { get; private set; }
        public RunningState Running { get; private set; }
        public CrouchingState Crouching { get; private set; }
        public AirState Air { get; private set; }

        void Awake()
        {
            Character = GetComponent<CharacterController>();

            Standing = new StandingState(this);
            Walking = new WalkingState(this);
            Running = new RunningState(this);
            Crouching = new CrouchingState(this);
            Air = new AirState(this);
        }

        void Start()
        {
            ChangeState(Standing);
        }

        void Update()
        {
            ApplyGravity();
            CurrentState.Update();
            MoveCharacter();
            JumpPressed = false;
        }

        public void ChangeState(AMovementState newState)
        {
            if (CurrentState == newState) return;
            
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
            
            OnStateChanged?.Invoke(CurrentState);
        }

        void ApplyGravity()
        {
            if (Character.isGrounded && VerticalVelocity < 0f)
                VerticalVelocity = -2f;

            VerticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        void MoveCharacter()
        {
            Vector3 velocity = HorizontalVelocity + Vector3.up * VerticalVelocity;
            Character.Move(velocity * Time.deltaTime);
        }

        public void SetInput(Vector2 input) => Input = input;
        public void SetRun(bool value) => RunHeld = value;
        public void SetCrouch(bool value) => CrouchHeld = value;
        public void PressJump() => JumpPressed = true;

        public void SetHorizontalVelocity(Vector3 target, float speed)
        {
            float accel = target.magnitude > 0.1f ? acceleration : deceleration;

            HorizontalVelocity = Vector3.MoveTowards(
                HorizontalVelocity,
                target * speed,
                accel * Time.deltaTime
            );
        }

        public void Jump()
        {
            VerticalVelocity = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            ChangeState(Air);
        }
    }
}

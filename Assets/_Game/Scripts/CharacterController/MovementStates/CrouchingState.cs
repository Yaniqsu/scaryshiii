namespace YNQ.Movement.States
{
    public class CrouchingState : AGroundedState
    {
        public CrouchingState(MovementController controller) : base(controller) { }

        public override void Enter()
        {
            //controller.Character.height = 1f;
        }

        public override void Exit()
        {
            //controller.Character.height = 1.8f;
        }

        public override void Update()
        {
            HandleGroundedTransitions();

            if (!controller.CrouchHeld)
            {
                controller.ChangeState(controller.Standing);
                return;
            }

            var dir = GetMoveDirection();
            controller.SetHorizontalVelocity(dir, controller.crouchSpeed);
        }
    }
}
namespace YNQ.Movement.States
{
    public class AirState : AMovementState
    {
        public AirState(MovementController controller) : base(controller) { }

        public override void Update()
        {
            if (controller.Character.isGrounded && controller.VerticalVelocity < 0)
            {
                controller.ChangeState(controller.Standing);
                return;
            }
            
            var dir = controller.transform.right * controller.Input.x +
                      controller.transform.forward * controller.Input.y;

            controller.SetHorizontalVelocity(dir, controller.walkSpeed);
        }
    }
}
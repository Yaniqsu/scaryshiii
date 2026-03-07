using UnityEngine;

namespace YNQ.Movement.States
{
    public class WalkingState : AGroundedState
    {
        public WalkingState(MovementController controller) : base(controller) { }

        public override void Update()
        {
            HandleGroundedTransitions();

            if (controller.Input.magnitude < 0.1f)
            {
                controller.ChangeState(controller.Standing);
                return;
            }
            
            if(controller.CrouchHeld)
            {
                controller.ChangeState(controller.Crouching);
                return;
            }

            var dir = GetMoveDirection();
            
            var forwardAmount = Vector3.Dot(
                dir.normalized,
                controller.transform.forward
            );

            if (forwardAmount > 0.7f && controller.RunHeld)
            {
                controller.ChangeState(controller.Running);
                return;
            }
            
            controller.SetHorizontalVelocity(dir, controller.walkSpeed);
        }
    }
}
using UnityEngine;

namespace YNQ.Movement.States
{
    public abstract class AGroundedState : AMovementState
    {
        protected AGroundedState(MovementController controller) : base(controller) { }
        
        protected void HandleGroundedTransitions()
        {
            if (!controller.Character.isGrounded)
            {
                controller.ChangeState(controller.Air);
                return;
            }

            if (controller.JumpPressed)
            {
                controller.Jump();
            }
        }

        protected Vector3 GetMoveDirection()
        {
            return controller.transform.right * controller.Input.x +
                   controller.transform.forward * controller.Input.y;
        }
    }
}
using UnityEngine;

namespace YNQ.Movement.States
{
    public class StandingState : AGroundedState
    {
        public StandingState(MovementController controller) : base(controller) { }

        public override void Update()
        {
            HandleGroundedTransitions();

            if (controller.CrouchHeld)
            {
                controller.ChangeState(controller.Crouching);
                return;
            }

            if (controller.Input.magnitude > 0.1f)
            {
                if(controller.RunHeld)
                    controller.ChangeState(controller.Running);
                else
                    controller.ChangeState(controller.Walking);

                return;
            }
            
            controller.SetHorizontalVelocity(Vector3.zero, 0);
        }
    }
}
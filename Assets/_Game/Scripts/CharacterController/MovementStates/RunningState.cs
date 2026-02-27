using UnityEngine;

namespace YNQ.Movement.States
{
    public class RunningState : AGroundedState
    {
        public RunningState(MovementController controller) : base(controller) { }

        public override void Update()
        {
            HandleGroundedTransitions();

            if (!controller.RunHeld)
            {
                controller.ChangeState(controller.Walking);
                return;
            }
            
            var dir = GetMoveDirection();
            var forwardAmount = Vector3.Dot(
                dir.normalized,
                controller.transform.forward
            );
            
            if (forwardAmount < 0.7f)
            {
                controller.ChangeState(controller.Walking);
                return;
            }
            
            var forwardDir = controller.transform.forward * forwardAmount;

            controller.SetHorizontalVelocity(forwardDir.normalized, controller.runSpeed);
        }
    }
}
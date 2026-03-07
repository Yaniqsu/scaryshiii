namespace YNQ.Movement.States
{
    public abstract class AMovementState
    {
        protected readonly MovementController controller;

        protected AMovementState(MovementController controller)
        {
            this.controller = controller;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}

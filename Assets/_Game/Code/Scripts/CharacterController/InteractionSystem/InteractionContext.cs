using UnityEngine;

namespace YNQ.InteractionSystem
{
    public struct InteractionContext
    {
        public Camera Camera;
        public Transform Player;

        public RaycastHit Hit;

        public Vector2 MouseDelta;
        public Vector3 GrabPointWorld;

        public float DeltaTime;
    }
}
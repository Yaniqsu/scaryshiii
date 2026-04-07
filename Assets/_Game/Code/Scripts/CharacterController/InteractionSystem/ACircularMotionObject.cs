using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YNQ.InteractionSystem
{
    public abstract class ACircularMotionObject : MonoBehaviour, IInteractable
    {
        [SerializeField] protected Vector3 rotationAxis;
        [SerializeField] protected Vector2 rotationLimits;
        [SerializeField] protected float torqueStrength;
        [SerializeField] protected float maxTorque;
        [SerializeField] protected float torqueSign;

        public InteractionType Type => InteractionType.Physics;
        public abstract InteractionTag Tag { get; }

        private Vector3 _rotation;

        private Queue<Vector2> _points = new();
        private int _maxPoints = 100;
        private Vector2 _lastMouseDir;
        private Vector2 _center;
        private float _smoothedInput;
        private float _inputVelocity;
        private bool _hasLastDir;

        private void Awake()
        {
            _rotation = transform.localEulerAngles;
        }


        public void BeginInteraction(InteractionContext context)
        {
            _hasLastDir = false;
        }

        public void InteractionUpdate(InteractionContext context)
        {
            _points.Enqueue(context.MousePos);
            if (_points.Count > _maxPoints)
                _points.Dequeue();
            _center = EstimateCenter(_points.ToArray());

            var currentDir = (context.MousePos - _center).normalized;

            if (!_hasLastDir)
            {
                _lastMouseDir = currentDir;
                _hasLastDir = true;
                return;
            }

            var angleDelta = Vector2.SignedAngle(_lastMouseDir, currentDir);
            _lastMouseDir = currentDir;

            _smoothedInput = Mathf.SmoothDamp(
                _smoothedInput,
                angleDelta,
                ref _inputVelocity,
                0.05f
            );

            float torqueAmount = Mathf.Clamp(
                _smoothedInput * torqueStrength,
                -maxTorque,
                maxTorque
            ) * torqueSign;

            _rotation += rotationAxis * torqueAmount;

            var finalRotation = new Vector3(
                Mathf.Clamp(_rotation.x, rotationLimits.x, rotationLimits.y),
                Mathf.Clamp(_rotation.y, rotationLimits.x, rotationLimits.y),
                Mathf.Clamp(_rotation.z, rotationLimits.x, rotationLimits.y));

            transform.localRotation = Quaternion.Euler(finalRotation);
            OnRotate(finalRotation);
        }

        public void EndInteraction()
        {
        }

        private static Vector2 EstimateCenter(Vector2[] points)
        {
            var sum = points.Aggregate(Vector2.zero, (current, p) => current + p);

            return sum / points.Length;
        }

        protected abstract void OnRotate(Vector3 rotation);
    }
}

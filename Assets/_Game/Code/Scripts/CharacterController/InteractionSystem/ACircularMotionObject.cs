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

        protected Vector3 rotation;

        private Queue<Vector2> _points = new();
        private int _maxPoints = 100;
        private Vector2 _lastMouseDir;
        private Vector2 _center;
        private float _smoothedInput;
        private float _inputVelocity;
        private bool _hasLastDir;

        protected virtual void Awake()
        {
            if (rotationLimits.x > rotationLimits.y)
            {
                (rotationLimits.x, rotationLimits.y) = (rotationLimits.y, rotationLimits.x);
            }
        }


        public void BeginInteraction(InteractionContext context)
        {
            _hasLastDir = false;
            _points.Clear();
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
                OnRotationBegin();
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

            if (Mathf.Abs(torqueAmount) <= 0.01f)
            {
                OnRotationEnd();
                _hasLastDir = false;
                return;
            }
            
            rotation += rotationAxis * torqueAmount;
            var localRotation = transform.localEulerAngles;

            var finalRotation = new Vector3(
                rotationAxis.x == 0 ? localRotation.x : Mathf.Clamp(rotation.x, rotationLimits.x, rotationLimits.y),
                rotationAxis.y == 0 ? localRotation.y : Mathf.Clamp(rotation.y, rotationLimits.x, rotationLimits.y),
                rotationAxis.z == 0 ? localRotation.z : Mathf.Clamp(rotation.z, rotationLimits.x, rotationLimits.y));

            transform.localRotation = Quaternion.Euler(finalRotation);
            rotation = finalRotation;
            
            OnRotate();
        }

        public void EndInteraction()
        {
            OnRotationEnd();
        }

        private static Vector2 EstimateCenter(Vector2[] points)
        {
            var sum = points.Aggregate(Vector2.zero, (current, p) => current + p);

            return sum / points.Length;
        }

        protected abstract void OnRotate();
        protected abstract void OnRotationBegin();
        protected abstract void OnRotationEnd();
    }
}

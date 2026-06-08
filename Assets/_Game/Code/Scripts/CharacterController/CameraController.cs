using System;
using System.Collections;
using UnityEngine;
using YNQ.Movement.States;

namespace YNQ.Movement
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private MovementController _controller;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float _lookSensitivity = 100f;
        [SerializeField] private float _shakeTransitionSpeed = 5f;
        [SerializeField] private Vector3 _standCameraPos;
        [SerializeField] private Vector3 _crouchCameraPos;
        [SerializeField] private float _posChangeDuration = 5f;
        [SerializeField] private AnimationCurve _posChangeCurve;

        [Header("ShakeValues")] 
        [SerializeField] private AnimationCurve shakeCurve = AnimationCurve.EaseInOut(-1, -1, 1, 1);
        [SerializeField] private AnimationCurve rotationShakeCurve = AnimationCurve.Linear(-1, -1, 1, 1);
        [SerializeField] private ShakeData standingShake;
        [SerializeField] private ShakeData walkingShake;
        [SerializeField] private ShakeData crouchingShake;
        [SerializeField] private ShakeData runningShake;
        
        private Vector2 _rotation = Vector3.zero;
        private Vector3 _cameraPos = Vector3.zero;
        private Vector3 _shakePosModifier = Vector3.zero;
        private Vector3 _shakeRotationModifier = Vector3.zero;
        private ShakeData _targetShake;
        private float _currentAmplitude = 0;
        private float _previousSinValue = 0;
        private float _currentFrequency = 1;
        private float _phase;
        
        private Coroutine _changeCameraHeightRoutine;
        
        public bool RotationLocked { get; set; }
        public event Action OnShakePhase;

        private void Start()
        {
            _rotation = new Vector2(0, 0);
            _targetShake = standingShake;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void Update()
        {
            ChangeShakeData();
            UpdateShakeValues();
            ShakeCamera();
            
            UpdateCameraPosBase();
            UpdateCameraRotationBase();;
        }

        private void LateUpdate()
        {
            UpdateCameraPosAddons();
            UpdateCameraRotationAddons();
        }

        private void ChangeShakeData()
        {
            var moveAmount = _controller.HorizontalVelocity.magnitude / _controller.runSpeed;

            _targetShake = _controller.CurrentState switch
            {
                StandingState => standingShake,
                WalkingState => walkingShake,
                RunningState => runningShake,
                CrouchingState when moveAmount > 0.1f => crouchingShake,
                _ => standingShake
            };
        }

        private void UpdateShakeValues()
        {
            _currentAmplitude = Mathf.Lerp(
                _currentAmplitude,
                _targetShake.amplitude,
                _shakeTransitionSpeed
            );

            _currentFrequency = Mathf.Lerp(
                _currentFrequency,
                _targetShake.frequency,
                _shakeTransitionSpeed
            );
        }

        private void ShakeCamera()
        {
            _phase += Time.deltaTime / _currentFrequency;

            var sin = shakeCurve.Evaluate(Mathf.Sin(_phase));
            var shakeSin = rotationShakeCurve.Evaluate(Mathf.Sin(_phase));

            var shakeValue = sin * _currentAmplitude;
            var rotationShakeValue = shakeSin * _currentAmplitude;
            
            if(sin < -0.9f && _previousSinValue > -0.9f)
                OnShakePhase?.Invoke();
            
            _shakePosModifier = new Vector3(
                shakeValue * _targetShake.multitude.x, 
                shakeValue * _targetShake.multitude.y, 
                shakeValue * _targetShake.multitude.z);
            
            _shakeRotationModifier = new Vector3(
                rotationShakeValue * _targetShake.rotationMultitude.x, 
                rotationShakeValue * _targetShake.rotationMultitude.y, 
                rotationShakeValue * _targetShake.rotationMultitude.z);
            
            _previousSinValue = sin;
        }

        private void UpdateCameraPosBase()
        {
            cameraTransform.localPosition = _cameraPos;
        }
        
        private void UpdateCameraRotationBase()
        {
            cameraTransform.localRotation = Quaternion.Euler(_rotation.y, 0, 0);;
        }
        
        private void UpdateCameraPosAddons()
        {
            cameraTransform.position += _shakePosModifier;
        }
        
        private void UpdateCameraRotationAddons()
        {
            cameraTransform.localRotation *= Quaternion.Euler(_shakeRotationModifier);
        }

        public void Rotate(Vector2 delta)
        {
            if (RotationLocked)
                return;
            
            _rotation.x += delta.x * _lookSensitivity * Time.deltaTime;
            _rotation.y -= delta.y * _lookSensitivity * Time.deltaTime;
            
            _rotation.y = Mathf.Clamp(_rotation.y, -90f, 90f);
            
            transform.localRotation = Quaternion.Euler(0, _rotation.x, 0);
        }

        public void OnStateChanged(AMovementState state)
        {
            var data = state switch
            {
                StandingState => standingShake,
                WalkingState => walkingShake,
                RunningState => runningShake,
                _ => standingShake
            };


            ChangeCameraPos(state is CrouchingState ? _crouchCameraPos : _standCameraPos);
        }

        private void ChangeCameraPos(Vector3 pos)
        {
            if (Vector3.Distance(cameraTransform.localPosition, pos) < 0.1f)
                return;
            
            if(_changeCameraHeightRoutine != null)
                StopCoroutine(_changeCameraHeightRoutine);

            _changeCameraHeightRoutine = StartCoroutine(ChangeCameraPosRoutine(pos, _posChangeDuration));
        }

        private IEnumerator ChangeCameraPosRoutine(Vector3 targetPos, float duration)
        {
            var startPos = cameraTransform.localPosition;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                _cameraPos = Vector3.Lerp(startPos, targetPos, _posChangeCurve.Evaluate(elapsed / duration));
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            _cameraPos = targetPos;
        }
    }
}
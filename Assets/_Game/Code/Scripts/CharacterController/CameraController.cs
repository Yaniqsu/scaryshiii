using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using YNQ.Movement.States;

namespace YNQ.Movement
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private MovementController _controller;
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private float _lookSensitivity = 100f;
        [SerializeField] private float _shakeTransitionSpeed = 5f;
        [SerializeField] private Vector3 _standCameraPos;
        [SerializeField] private Vector3 _crouchCameraPos;
        [SerializeField] private float _posChangeDuration = 5f;
        [SerializeField] private AnimationCurve _posChangeCurve;

        [Header("ShakeValues")] 
        [SerializeField] private CameraShakeValues _standingShake;
        [SerializeField] private CameraShakeValues _walkingShake;
        [SerializeField] private CameraShakeValues _crouchingShake;
        [SerializeField] private CameraShakeValues _runningShake;
        
        private CinemachineBasicMultiChannelPerlin _perlin;
        
        private Vector2 _rotation;
        private float _targetAmplitude;
        private float _targetFrequency;
        
        private Coroutine _changeCameraHeightRoutine;
        
        public bool RotationLocked { get; set; }

        private void Start()
        {
            _rotation = new Vector2(0, 0);
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _perlin = _camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
        
        void Update()
        {
            if (_perlin == null) return;
            
            float moveAmount = _controller.HorizontalVelocity.magnitude / _controller.runSpeed;

            var values = _controller.CurrentState switch
            {
                StandingState => _standingShake,
                WalkingState => _walkingShake,
                RunningState => _runningShake,
                CrouchingState when moveAmount > 0.1f => _crouchingShake,
                CrouchingState when moveAmount < .1f => _standingShake,
                _ => new CameraShakeValues(0f, 0f)
            };

            _targetAmplitude = values.amplitude;
            _targetFrequency = values.frequency;

            _perlin.AmplitudeGain = Mathf.Lerp(
                _perlin.AmplitudeGain,
                _targetAmplitude,
                _shakeTransitionSpeed * Time.deltaTime
            );

            _perlin.FrequencyGain = Mathf.Lerp(
                _perlin.FrequencyGain,
                _targetFrequency,
                _shakeTransitionSpeed * Time.deltaTime
            );
        }

        public void Rotate(Vector2 delta)
        {
            if (RotationLocked)
                return;
            
            _rotation.x += delta.x * _lookSensitivity * Time.deltaTime;
            _rotation.y -= delta.y * _lookSensitivity * Time.deltaTime;
            
            _rotation.y = Mathf.Clamp(_rotation.y, -90f, 90f);
            
            _camera.transform.localRotation = Quaternion.Euler(_rotation.y, 0, 0);
            transform.localRotation = Quaternion.Euler(0, _rotation.x, 0);
        }

        public void OnStateChanged(AMovementState state)
        {
            CameraShakeValues values = state switch
            {
                StandingState => _standingShake,
                WalkingState => _walkingShake,
                RunningState => _runningShake,
                _ => new CameraShakeValues(0f, 0f)
            };


            ChangeCameraPos(state is CrouchingState ? _crouchCameraPos : _standCameraPos);

            _targetAmplitude = values.amplitude;
            _targetFrequency = values.frequency;
        }

        private void ChangeCameraPos(Vector3 pos)
        {
            if (Vector3.Distance(_camera.transform.localPosition, pos) < 0.1f)
                return;
            
            if(_changeCameraHeightRoutine != null)
                StopCoroutine(_changeCameraHeightRoutine);

            _changeCameraHeightRoutine = StartCoroutine(ChangeCameraPosRoutine(pos, _posChangeDuration));
        }

        private IEnumerator ChangeCameraPosRoutine(Vector3 targetPos, float duration)
        {
            var startPos = _camera.transform.localPosition;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                _camera.transform.localPosition = Vector3.Lerp(startPos, targetPos, _posChangeCurve.Evaluate(elapsed / duration));
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            _camera.transform.localPosition = targetPos;
        }
    }
}
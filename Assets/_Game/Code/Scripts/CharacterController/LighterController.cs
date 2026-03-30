using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YNQ.Player
{
    public class LighterController : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private float lightChance;
        [SerializeField] private HandLighter lighterPrefab;
        [SerializeField] private Light lighterLight;
        [SerializeField] private float didLightHideTime;
        [SerializeField] private float didntLightHideTime;
        [Header("Animation")] 
        [SerializeField] private float lightIntensityMin;
        [SerializeField] private float lightIntensityMax;
        [SerializeField] private float animationTime;
        [Header("DEBUG")] 
        [SerializeField] private bool enableBeDefault;

        private HandController _handController;
        private HandLighter _lighter;
        private Coroutine _lightRoutine;
        public bool LighterOn { get; private set; }
        
        public bool Active { get; private set; } = false;
        

        private void Awake()
        {
            _handController = GetComponent<HandController>();

        }

        private void Start()
        {
            if(enableBeDefault)
                Enable();
        }

        private void Update()
        {
            lighterLight.intensity = Mathf.Lerp(
                lightIntensityMin, lightIntensityMax, (Mathf.Sin(Time.time * animationTime) * Mathf.Sin(-Time.time * animationTime) + 1) / 2);
        }

        public void Enable()
        {
            if (Active)
                return;
            
            Active = true;
            _lighter = Instantiate(lighterPrefab, transform);
            _handController.OccupyLeftHand(_lighter.gameObject, false);
        }

        public void TryLight()
        {
            if (LighterOn)
            {
                HideLighter(0);
                return;
            }
            
            _handController.ToggleLeftHand(true);
            _lighter.TryLight();
            
            if (Random.Range(0, 100) < lightChance)
                Light();
            else
                HideLighter(didntLightHideTime);
        }

        private void HideLighter(float delayTime)
        {
            if(_lightRoutine != null)
                StopCoroutine(_lightRoutine);

            _lightRoutine = StartCoroutine(HideLighterRoutine(delayTime));
        }

        private IEnumerator HideLighterRoutine(float time)
        {
            yield return new WaitForSeconds(time);
            
            Extinguish();

            yield return new WaitForSeconds(1);
            
            _handController.ToggleLeftHand(false);
        }

        private void Light()
        {
            LighterOn = true;
            
            _lighter.Light();
            lighterLight.enabled = true;
            
            HideLighter(didLightHideTime);
        }

        private void Extinguish()
        {
            LighterOn = false;

            lighterLight.enabled = false;
            _lightRoutine = null;
            _lighter.Extinguish();
        }
    }
}
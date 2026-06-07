using FMOD.Studio;
using UnityEngine;
using YNQ.Movement;
using YNQ.Movement.States;

public class CharacterAudioController : MonoBehaviour
{
    [Header("General")] 
    [SerializeField] private SoundBank soundBank;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private MovementController movementController;
    [SerializeField] private SurfaceController surfaceController;

    [Header("Footsteps")] 
    [SerializeField, SoundName(nameof(soundBank))] private string footstepsSound;
    [SerializeField, SoundName(nameof(soundBank))] private string crouchSound;
    [SerializeField, SoundName(nameof(soundBank))] private string standUpSound;

    private EventInstance _footstepsInstance;

    private void Start()
    {
        _footstepsInstance = AudioManager.instance.CreateInstance(soundBank[footstepsSound]);
        cameraController.OnShakePhase += PlayFootstepsAudio;
        movementController.OnCrouchChanged.AddListener(OnCrouchChanged);
    }

    private void OnCrouchChanged(bool crouching)
    {
        AudioManager.PlayOneShot(soundBank[crouching ? crouchSound : standUpSound], transform.position);
    }


    private void PlayFootstepsAudio()
    {
        if (!movementController.Moving)
            return;
        
        var label = surfaceController.CurrentSurface.ToString();
        var speed = movementController.HorizontalVelocity.magnitude / movementController.runSpeed;
        
        _footstepsInstance.setParameterByNameWithLabel("surface_type", label);
        _footstepsInstance.setParameterByName("move_speed", speed);
        _footstepsInstance.start();
    }

}

using UnityEngine;
using YNQ.Movement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementController movementController;
    [SerializeField] private SurfaceController surfaceController;

    private void Start()
    {
        movementController.OnMoveChanged.AddListener(ToggleSurfaceCheck);
    }

    private void ToggleSurfaceCheck(bool active)
    {
        if(active)
        {
            surfaceController.BeginCheck();
        }
        else
        {
            surfaceController.EndCheck();
        }
    }
}

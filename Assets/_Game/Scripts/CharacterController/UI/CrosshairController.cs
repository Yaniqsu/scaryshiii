using UnityEngine;
using UnityEngine.UI;
using YNQ.InteractionSystem;

namespace YNQ.Character
{
    public class CrosshairController : MonoBehaviour
    {
        [SerializeField] private CrosshairData data;
        [SerializeField] private Image crosshair;
        [SerializeField] private InteractionController interactionController;

        private void Start()
        {
            ChangeCrosshair(CrosshairType.Default);
            
            interactionController.onPhysicsInteractableFound.AddListener(() => ChangeCrosshair(CrosshairType.Interact));
            interactionController.onInteractableLost.AddListener(() => ChangeCrosshair(CrosshairType.PickUp));
            interactionController.onInteractableLost.AddListener(() => ChangeCrosshair(CrosshairType.Default));
        }

        public void ChangeCrosshair(CrosshairType type)
        {
            crosshair.sprite = data.GetSprite(type);
        }
    }
}

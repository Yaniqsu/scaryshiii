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
            crosshair.sprite = data.DefaultSprite;
            
            interactionController.onInteractableFound.AddListener(i => ChangeCrosshair(i.Tag));
            interactionController.onInteractableLost.AddListener(() => crosshair.sprite = data.DefaultSprite);
        }

        public void ChangeCrosshair(InteractionTag interactionTag)
        {
            crosshair.sprite = data.GetSprite(interactionTag);
        }
    }
}

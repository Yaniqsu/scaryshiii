using System.Linq;
using UnityEngine;
using YNQ.InteractionSystem;
using YNQ.Utils;

namespace YNQ.Character
{
    [CreateAssetMenu(fileName = "Crosshair Data", menuName = "Scriptable Objects /Crosshair Data")]
    public class CrosshairData : ScriptableObject
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private TableRow<InteractionTag, Sprite>[] data;
        
        public Sprite DefaultSprite => defaultSprite;

        public Sprite GetSprite(InteractionTag type)
        {
            return data.Any(d => d.item1 == type) ? 
                    data.First(d => d.item1 == type).item2 
                    : defaultSprite;
        }
    }
}
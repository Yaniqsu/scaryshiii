using System.Linq;
using UnityEngine;
using YNQ.Utils;

namespace YNQ.Character
{
    [CreateAssetMenu(fileName = "Crosshair Data", menuName = "Scriptable Objects /Crosshair Data")]
    public class CrosshairData : ScriptableObject
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private TableRow<CrosshairType, Sprite>[] data;

        public Sprite GetSprite(CrosshairType type)
        {
            return data.Any(d => d.item1 == type) ? 
                    data.First(d => d.item1 == type).item2 
                    : defaultSprite;
        }
    }
}
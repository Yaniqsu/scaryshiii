using UnityEngine;

namespace YNQ.ItemsDisplacer
{
    [CreateAssetMenu(fileName = "Tier Icon Bank", menuName = "Scriptable Objects /Items Displacer /Tier Icon Bank")]
    public class TierIconBank : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Tier0 {get; private set;}
        
        [field: SerializeField]
        public Sprite Tier1 {get; private set;}
        
        [field: SerializeField]
        public Sprite Tier2 {get; private set;}
        
        [field: SerializeField]
        public Sprite Tier3 {get; private set;}
    }
}
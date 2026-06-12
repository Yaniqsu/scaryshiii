using UnityEngine;

namespace YNQ.ItemsDisplacer
{
    public class ItemSpawnPoint : MonoBehaviour
    {
        [field: SerializeField]
        public EItemTier ItemTier { get; private set; }
        
        [field: SerializeField]
        public EItemSize ItemSize { get; private set; }
    }
}

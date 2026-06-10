using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace YNQ.AreaTrigger
{
    public class AreaTrigger : MonoBehaviour
    {
        [SerializeField, Tag] private string[] triggerTags;
        [SerializeField] private bool countTriggers = true;
        [ShowIf(nameof(countTriggers)), SerializeField, Min(1)] private int maxTriggerTimes = 1;
        public bool triggerEnabled = true;
    
        public UnityEvent<Collider> TriggerEnter;
        public UnityEvent<Collider> TriggerExit;

        private int _triggerCount;


        private void OnTriggerEnter(Collider other)
        {
            if (!triggerEnabled)
                return;
        
            if((!countTriggers || _triggerCount < maxTriggerTimes) && triggerTags.Contains(other.tag))
            {
                _triggerCount++;
                TriggerEnter.Invoke(other);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (!triggerEnabled)
                return;
        
            if(triggerTags.Contains(other.tag))
                TriggerExit.Invoke(other);
        }
    }
}

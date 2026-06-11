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
        private int _overlaps;
        private bool _triggered;


        private void OnTriggerEnter(Collider other)
        {
            if (!triggerEnabled)
                return;
        
            if((!countTriggers || _triggerCount < maxTriggerTimes) && triggerTags.Contains(other.tag))
            {
                ModifyOverlaps(other, 1);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (!triggerEnabled)
                return;
        
            if(triggerTags.Contains(other.tag))
                ModifyOverlaps(other, -1);
        }

        private void ModifyOverlaps(Collider other, int modifier)
        {
            _overlaps = Mathf.Max(0, _overlaps + modifier);
            
            if(_overlaps > 0 && !_triggered)
                NotifyTriggerEnter(other);
            
            if(_overlaps == 0 && _triggered)
                NotifyTriggerExit(other);
        }

        private void NotifyTriggerEnter(Collider other)
        {
            _triggered = true;
            _triggerCount++;
            TriggerEnter.Invoke(other);
        }

        private void NotifyTriggerExit(Collider other)
        {
            _triggered = false;
            TriggerExit.Invoke(other);
        }
    }
}

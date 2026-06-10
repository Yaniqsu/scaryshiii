using System.Collections.Generic;
using UnityEngine;

namespace YNQ.Environment
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private bool buildingVisible;
        [SerializeField] private bool propsVisible;
        [SerializeField] private GameObject building;
        [SerializeField] private GameObject props;
        [SerializeField] private Room[] roomBuildingsAdjacent;
        [SerializeField] private Room[] roomPropsAdjacent;

        private readonly HashSet<object> _buildingRequests = new();
        private readonly HashSet<object> _propsRequests = new();
        private bool _roomActive;

        private void OnValidate()
        {
            Toggle();
        }

        public void OnRoomEnter()
        {
            _roomActive = true;
            
            foreach (var room in roomBuildingsAdjacent)
            {
                room.AddBuildingRequest(this);
            }
            
            foreach (var room in roomPropsAdjacent)
            {
                room.AddPropsRequest(this);
            }
        }

        public void OnRoomExit()
        {
            _roomActive = false;
            
            foreach (var room in roomBuildingsAdjacent)
            {
                room.RemoveBuildingRequest(this);
            }
            
            foreach (var room in roomPropsAdjacent)
            {
                room.RemovePropsRequest(this);
            }
        }

        private void AddBuildingRequest(object request)
        {
            _buildingRequests.Add(request);
            UpdateVisibility();
        }
        
        private void AddPropsRequest(object request)
        {
            _propsRequests.Add(request);
            UpdateVisibility();
        }

        private void RemoveBuildingRequest(object request)
        {
            _buildingRequests.Remove(request);
            UpdateVisibility();
        }
        
        private void RemovePropsRequest(object request)
        {
            _propsRequests.Remove(request);
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            if (_roomActive)
                return;
            
            buildingVisible = _buildingRequests.Count > 0;
            propsVisible = _propsRequests.Count > 0;
            Toggle();
        }

        private void Toggle()
        {
            building.SetActive(buildingVisible);
            props.SetActive(propsVisible);
        }
    }
}

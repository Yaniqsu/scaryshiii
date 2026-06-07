using System;
using UnityEngine;

namespace YNQ.Movement
{
    [Serializable]
    public struct ShakeData
    {
        public float amplitude;
        public float frequency;
        public Vector3 multitude;
        public Vector3 rotationMultitude;
    }
}
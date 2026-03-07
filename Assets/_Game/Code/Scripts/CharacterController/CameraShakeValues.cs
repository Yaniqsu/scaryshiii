using System;

namespace YNQ.Movement
{
    [Serializable]
    public struct CameraShakeValues
    {
        public float amplitude;
        public float frequency;

        public CameraShakeValues(float amplitude, float frequency)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
        }
    }
}
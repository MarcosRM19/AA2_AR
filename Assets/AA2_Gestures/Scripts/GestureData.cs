using UnityEngine;
using UnityEngine.Events;


namespace AA2Gesture_G4
{
    [CreateAssetMenu(fileName = "NewGesture", menuName = "AA2 Gesture/Gesture")]
    public class GestureData : ScriptableObject
    {
        public string gestureName;

        [Range(0f, 1f)] public float thumbCurl;
        [Range(0f, 1f)] public float indexCurl;
        [Range(0f, 1f)] public float middleCurl;
        [Range(0f, 1f)] public float ringCurl;
        [Range(0f, 1f)] public float littleCurl;

        public float threshold = 0.2f;

        public UnityEvent onGestureDetected;
    }
}


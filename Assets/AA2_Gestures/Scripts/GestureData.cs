using UnityEngine;

[CreateAssetMenu(fileName = "NewGesture", menuName = "AA2 Inventory/Gesture")]
public class GestureData : ScriptableObject
{
    public string gestureName;

    [Range(0f, 1f)] public float thumbCurl;
    [Range(0f, 1f)] public float indexCurl;
    [Range(0f, 1f)] public float middleCurl;
    [Range(0f, 1f)] public float ringCurl;
    [Range(0f, 1f)] public float littleCurl;

    public float threshold = 0.2f;
}

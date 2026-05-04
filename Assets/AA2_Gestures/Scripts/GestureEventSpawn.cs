using TMPro;
using UnityEngine;

namespace AA2Gesture_G4
{
    public class GestureEventSpawn : GestureEvent
    {
        [SerializeField] private TextMeshProUGUI _debugText;

        protected override void OnGestureTriggered()
        {
            Debug.Log("Gesto detectado: " + _gesture.gestureName);
            _debugText.text = "Gesto de Spawn de Objeto detectado: " + _gesture.gestureName;
        }
    }
}

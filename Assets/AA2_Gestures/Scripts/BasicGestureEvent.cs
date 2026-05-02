using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class BasicGestureEvent : GestureEvent
{
    [SerializeField] private TextMeshProUGUI _debugText;

    protected override void OnGestureTriggered()
    {
        Debug.Log("Gesto detectado: " + _gesture.gestureName);
        _debugText.text = "Gesto detectado: " + _gesture.gestureName;
    }
}

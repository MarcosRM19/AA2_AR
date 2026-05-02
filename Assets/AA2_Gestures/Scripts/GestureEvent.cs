using System.Collections;
using UnityEngine;

public abstract class GestureEvent : MonoBehaviour
{
    [SerializeField] protected GestureData _gesture;

    protected virtual void OnEnable()
    {
        if (_gesture == null)
        {
            Debug.LogWarning($"{gameObject.name}: no tiene GestureData asignado.");
            return;
        }
        _gesture.onGestureDetected.AddListener(OnGestureTriggered);
        StartCoroutine(RegisterWhenReady());
    }

    private IEnumerator RegisterWhenReady()
    {
        yield return new WaitUntil(() => GestureReader.Instance != null);
        GestureReader.Instance.AddGesture(_gesture);
        Debug.Log($"Registrado: {_gesture.gestureName}");
    }

    protected virtual void OnDisable()
    {
        if (_gesture == null) 
            return;

        GestureReader.Instance?.RemoveGesture(_gesture);
        _gesture.onGestureDetected.RemoveListener(OnGestureTriggered);
    }

    protected virtual void OnDestroy()
    {
        if (_gesture == null) 
            return;

        _gesture.onGestureDetected.RemoveListener(OnGestureTriggered);
    }

    protected abstract void OnGestureTriggered();
}
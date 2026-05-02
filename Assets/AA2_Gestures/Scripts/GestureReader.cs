using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.Events;
using TMPro;

public class GestureReader : MonoBehaviour
{
    public static GestureReader Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private float _detectionInterval = 0.1f;
    private List<GestureData> _gestures = new();

    private XRHandSubsystem _handSubsystem;
    private float _timer;
    private string _lastDetectedGesture = "";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
            _handSubsystem = subsystems[0];
        else
            Debug.LogWarning("GestureReader: No se encontró XRHandSubsystem.");
    }

    void Update()
    {
        if (_handSubsystem == null) return;

        _timer += Time.deltaTime;
        if (_timer < _detectionInterval) return;
        _timer = 0f;

        if (!CheckHand(_handSubsystem.rightHand))
            CheckHand(_handSubsystem.leftHand);
    }

    private bool CheckHand(XRHand hand)
    {
        if (!hand.isTracked) return false;

        float[] curls = GetFingerCurls(hand);
        if (curls == null) return false;

        return DetectGesture(curls);
    }

    private float[] GetFingerCurls(XRHand hand)
    {
        float thumb = GetFingerCurl(hand, XRHandFingerID.Thumb);
        float index = GetFingerCurl(hand, XRHandFingerID.Index);
        float middle = GetFingerCurl(hand, XRHandFingerID.Middle);
        float ring = GetFingerCurl(hand, XRHandFingerID.Ring);
        float little = GetFingerCurl(hand, XRHandFingerID.Little);

        if (thumb < 0 || index < 0 || middle < 0 || ring < 0 || little < 0)
            return null;

        return new float[] { thumb, index, middle, ring, little };
    }

    private float GetFingerCurl(XRHand hand, XRHandFingerID finger)
    {
        XRHandJointID proximal = GetProximalJoint(finger);
        XRHandJointID tip = GetTipJoint(finger);

        if (!hand.GetJoint(proximal).TryGetPose(out Pose proximalPose)) 
            return -1f;
        if (!hand.GetJoint(tip).TryGetPose(out Pose tipPose)) 
            return -1f;
        if (!hand.GetJoint(XRHandJointID.Wrist).TryGetPose(out Pose wristPose)) 
            return -1f;

        float extended = Vector3.Distance(proximalPose.position, wristPose.position);
        float current = Vector3.Distance(tipPose.position, wristPose.position);

        return Mathf.Clamp01(1f - (current / (extended * 1.8f)));
    }

    private bool DetectGesture(float[] curls)
    {
        foreach (var gesture in _gestures)
        {
            if (gesture == null) continue;

            float[] targets = {
            gesture.thumbCurl,
            gesture.indexCurl,
            gesture.middleCurl,
            gesture.ringCurl,
            gesture.littleCurl
        };

            bool match = true;
            for (int i = 0; i < 5; i++)
            {
                if (Mathf.Abs(curls[i] - targets[i]) > gesture.threshold)
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                if (_lastDetectedGesture != gesture.gestureName)
                {
                    _lastDetectedGesture = gesture.gestureName;
                    gesture.onGestureDetected?.Invoke();
                }
                return true; 
            }
        }

        _lastDetectedGesture = "";
        return false;
    }

    private XRHandJointID GetProximalJoint(XRHandFingerID finger)
    {
        switch (finger)
        {
            case XRHandFingerID.Thumb: 
                return XRHandJointID.ThumbProximal;
            case XRHandFingerID.Index: 
                return XRHandJointID.IndexProximal;
            case XRHandFingerID.Middle: 
                return XRHandJointID.MiddleProximal;
            case XRHandFingerID.Ring: 
                return XRHandJointID.RingProximal;
            case XRHandFingerID.Little: 
                return XRHandJointID.LittleProximal;
            default: 
                return XRHandJointID.Wrist;
        }
    }

    private XRHandJointID GetTipJoint(XRHandFingerID finger)
    {
        switch (finger)
        {
            case XRHandFingerID.Thumb: 
                return XRHandJointID.ThumbTip;
            case XRHandFingerID.Index: 
                return XRHandJointID.IndexTip;
            case XRHandFingerID.Middle: 
                return XRHandJointID.MiddleTip;
            case XRHandFingerID.Ring: 
                return XRHandJointID.RingTip;
            case XRHandFingerID.Little: 
                return XRHandJointID.LittleTip;
            default: 
                return XRHandJointID.Wrist;
        }
    }

    public void AddGesture(GestureData data)
    {
        _gestures.Add(data);
    }

    public void RemoveGesture(GestureData data)
    {
        _gestures.Remove(data);
    }
}

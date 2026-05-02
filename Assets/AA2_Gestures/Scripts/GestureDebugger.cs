using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Hands;

public class GestureDebugger : MonoBehaviour
{
    public TextMeshProUGUI debugText; 

    private XRHandSubsystem handSubsystem;

    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
            handSubsystem = subsystems[0];
    }

    void Update()
    {
        if (handSubsystem == null || debugText == null) return;

        string info = "";
        info += FormatHand(handSubsystem.rightHand, "RIGHT");
        info += FormatHand(handSubsystem.leftHand, "LEFT");
        debugText.text = info;
    }

    private string FormatHand(XRHand hand, string label)
    {
        if (!hand.isTracked) return label + ": no detectada\n\n";

        return $"{label}\n" +
               $"Thumb:  {GetCurl(hand, XRHandFingerID.Thumb):F2}\n" +
               $"Index:  {GetCurl(hand, XRHandFingerID.Index):F2}\n" +
               $"Middle: {GetCurl(hand, XRHandFingerID.Middle):F2}\n" +
               $"Ring:   {GetCurl(hand, XRHandFingerID.Ring):F2}\n" +
               $"Little: {GetCurl(hand, XRHandFingerID.Little):F2}\n\n";
    }

    private float GetCurl(XRHand hand, XRHandFingerID finger)
    {
        XRHandJointID proximal = GetProximalJoint(finger);
        XRHandJointID tip = GetTipJoint(finger);

        if (!hand.GetJoint(proximal).TryGetPose(out Pose proximalPose)) return -1f;
        if (!hand.GetJoint(tip).TryGetPose(out Pose tipPose)) return -1f;
        if (!hand.GetJoint(XRHandJointID.Wrist).TryGetPose(out Pose wristPose)) return -1f;

        float extended = Vector3.Distance(proximalPose.position, wristPose.position);
        float current = Vector3.Distance(tipPose.position, wristPose.position);
        return Mathf.Clamp01(1f - (current / (extended * 1.8f)));
    }

    private XRHandJointID GetProximalJoint(XRHandFingerID finger)
    {
        switch (finger)
        {
            case XRHandFingerID.Thumb: return XRHandJointID.ThumbProximal;
            case XRHandFingerID.Index: return XRHandJointID.IndexProximal;
            case XRHandFingerID.Middle: return XRHandJointID.MiddleProximal;
            case XRHandFingerID.Ring: return XRHandJointID.RingProximal;
            case XRHandFingerID.Little: return XRHandJointID.LittleProximal;
            default: return XRHandJointID.Wrist;
        }
    }

    private XRHandJointID GetTipJoint(XRHandFingerID finger)
    {
        switch (finger)
        {
            case XRHandFingerID.Thumb: return XRHandJointID.ThumbTip;
            case XRHandFingerID.Index: return XRHandJointID.IndexTip;
            case XRHandFingerID.Middle: return XRHandJointID.MiddleTip;
            case XRHandFingerID.Ring: return XRHandJointID.RingTip;
            case XRHandFingerID.Little: return XRHandJointID.LittleTip;
            default: return XRHandJointID.Wrist;
        }
    }
}

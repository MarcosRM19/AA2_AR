using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

public class GestureDebugger : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;

    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
            handSubsystem = subsystems[0];
    }

    void OnGUI()
    {
        if (handSubsystem == null) return;

        DrawHand(handSubsystem.rightHand, "RIGHT", 10);
        DrawHand(handSubsystem.leftHand, "LEFT", 130);
    }

    private void DrawHand(XRHand hand, string label, int yOffset)
    {
        if (!hand.isTracked)
        {
            GUI.Label(new Rect(10, yOffset, 300, 20), label + ": no detectada");
            return;
        }

        GUI.Label(new Rect(10, yOffset, 300, 20), label);
        GUI.Label(new Rect(10, yOffset + 20, 300, 20), $"Thumb:  {GetCurl(hand, XRHandFingerID.Thumb):F2}");
        GUI.Label(new Rect(10, yOffset + 40, 300, 20), $"Index:  {GetCurl(hand, XRHandFingerID.Index):F2}");
        GUI.Label(new Rect(10, yOffset + 60, 300, 20), $"Middle: {GetCurl(hand, XRHandFingerID.Middle):F2}");
        GUI.Label(new Rect(10, yOffset + 80, 300, 20), $"Ring:   {GetCurl(hand, XRHandFingerID.Ring):F2}");
        GUI.Label(new Rect(10, yOffset + 100, 300, 20), $"Little: {GetCurl(hand, XRHandFingerID.Little):F2}");
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

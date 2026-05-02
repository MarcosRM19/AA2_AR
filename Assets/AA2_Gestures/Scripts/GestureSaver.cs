using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR;
using TMPro;
using System.Collections;

public class GestureSaver : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    private UnityEngine.XR.InputDevice rightDevice;
    private UnityEngine.XR.InputDevice leftDevice;
    private bool wasPressed = false;
    public TextMeshProUGUI debugText;
    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
            handSubsystem = subsystems[0];
    }

    void Update()
    {
        if (!rightDevice.isValid)
            rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (!leftDevice.isValid)
            leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        rightDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool rightPressed);
        leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool leftPressed);
        bool pressed = rightPressed || leftPressed;

        if (pressed && !wasPressed)
            TrySaveGesture();

        wasPressed = pressed;
    }

    private void TrySaveGesture()
    {
        if (handSubsystem == null) return;

        XRHand hand = handSubsystem.rightHand.isTracked
            ? handSubsystem.rightHand
            : handSubsystem.leftHand;

        if (!hand.isTracked)
        {
            Debug.LogWarning("GestureSaver: ninguna mano detectada.");
            return;
        }

        float thumb = GetCurl(hand, XRHandFingerID.Thumb);
        float index = GetCurl(hand, XRHandFingerID.Index);
        float middle = GetCurl(hand, XRHandFingerID.Middle);
        float ring = GetCurl(hand, XRHandFingerID.Ring);
        float little = GetCurl(hand, XRHandFingerID.Little);

        SaveGesture(thumb, index, middle, ring, little);
    }

    private void SaveGesture(float thumb, float index, float middle, float ring, float little)
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, "gestures.txt");

        string entry = $"------\n" +
                       $"Thumb:  {thumb:F2}\n" +
                       $"Index:  {index:F2}\n" +
                       $"Middle: {middle:F2}\n" +
                       $"Ring:   {ring:F2}\n" +
                       $"Little: {little:F2}\n\n";

        System.IO.File.AppendAllText(path, entry);
        debugText.text = "Gesto guardado";
        StartCoroutine(ClearText());
    }

    private IEnumerator ClearText()
    {
        yield return new WaitForSeconds(1f);
        debugText.text = "";
    }

    private float GetCurl(XRHand hand, XRHandFingerID finger)
    {
        XRHandJointID proximal = GetProximalJoint(finger);
        XRHandJointID tip = GetTipJoint(finger);

        if (!hand.GetJoint(proximal).TryGetPose(out Pose proximalPose)) return 0f;
        if (!hand.GetJoint(tip).TryGetPose(out Pose tipPose)) return 0f;
        if (!hand.GetJoint(XRHandJointID.Wrist).TryGetPose(out Pose wristPose)) return 0f;

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

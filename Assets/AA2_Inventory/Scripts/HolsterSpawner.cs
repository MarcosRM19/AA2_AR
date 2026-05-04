using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class HolsterSpawner : MonoBehaviour
{
    [Header("XR References")]
    [SerializeField] private Transform _cameraOffset;

    [Header("Input")]
    [SerializeField] private InputActionReference _spawnActionRight;
    [SerializeField] private InputActionReference _spawnActionLeft;

    [Header("Spawn")]
    [SerializeField] private GameObject _holsterPrefab;
    [SerializeField] private float _holdTime;

    [Header("Body Detection")]
    [SerializeField] private Transform _bodyAnchor;
    [SerializeField] private float _bodyRadius;

    [Header("Feedback")]
    [SerializeField] private float[] _hapticSteps;

    [Header("Controller Transforms")]
    [SerializeField] private Transform _rightControllerTransform;
    [SerializeField] private Transform _leftControllerTransform;

    private float _holdTimer    = 0f;
    private bool  _isHolding    = false;
    private bool  _hapticsFired = false;

    private UnityEngine.XR.InputDevice _rightDevice;
    private UnityEngine.XR.InputDevice _leftDevice;

    void OnEnable()
    {
        _spawnActionRight?.action.Enable();
        _spawnActionLeft?.action.Enable();
    }

    void OnDisable()
    {
        _spawnActionRight?.action.Disable();
        _spawnActionLeft?.action.Disable();
    }

    void Update()
    {
        if (!_rightDevice.isValid)
            _rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (!_leftDevice.isValid)
            _leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        float right = _spawnActionRight?.action.ReadValue<float>() ?? 0f;
        float left = _spawnActionLeft?.action.ReadValue<float>() ?? 0f;
        bool pressed = right > 0.8f || left > 0.8f;
        bool rightActive = right > 0.8f;

        if (pressed)
        {
            if (!_isHolding)
            {
                _isHolding = true;
                _holdTimer = 0f;
                _hapticsFired = false;
            }

            _holdTimer += Time.deltaTime;

            foreach (float step in _hapticSteps)
            {
                if (_holdTimer >= step && _holdTimer - Time.deltaTime < step)
                    SendHaptic(rightActive ? _rightDevice : _leftDevice, 0.3f, 0.05f);
            }

            if (_holdTimer >= _holdTime && !_hapticsFired)
            {
                _hapticsFired = true;
                SpawnHolster(rightActive);
                SendHaptic(rightActive ? _rightDevice : _leftDevice, 0.8f, 0.2f);
            }
        }
        else
        {
            _isHolding = false;
            _holdTimer = 0f;
        }
    }

    void SendHaptic(UnityEngine.XR.InputDevice device, float amplitude, float duration)
    {
        if (device.isValid)
            device.SendHapticImpulse(0, amplitude, duration);
    }

    void SpawnHolster(bool rightActive)
    {
        Transform controllerTransform = rightActive ? _rightControllerTransform : _leftControllerTransform;
        Vector3 worldPos = controllerTransform.position;
        Quaternion worldRot = controllerTransform.rotation;

        GameObject holsterGO = Instantiate(_holsterPrefab, worldPos, worldRot);

        var slot = holsterGO.GetComponent<HolsterSlot>();
        if (slot == null) return;

        bool isBodyHolster = IsInsideBodyCapsule(worldPos);

        if (isBodyHolster)
        {
            holsterGO.transform.SetParent(_bodyAnchor);

            slot.SetWeaponUpAxis(holsterGO.transform.InverseTransformDirection(controllerTransform.forward));
            slot.SetWeaponRightAxis(holsterGO.transform.InverseTransformDirection(controllerTransform.right));
            slot.SetHolsterType(HolsterType.Body);
        }
        else
        {
            holsterGO.transform.SetParent(null);

            slot.SetWeaponUpAxis(controllerTransform.forward);
            slot.SetWeaponRightAxis(controllerTransform.right);
            slot.SetHolsterType(HolsterType.World);
        }

        HolsterManager.Instance.RegisterHolster(slot);
    }

    private bool IsInsideBodyCapsule(Vector3 point)
    {
        if (_bodyAnchor == null || _cameraOffset == null) 
            return false;

        Vector3 top = _cameraOffset.position;
        Vector3 bottom = _bodyAnchor.position - (_cameraOffset.position - _bodyAnchor.position);

        Vector3 ab = top - bottom;
        Vector3 ap = point - bottom;
        float t = Mathf.Clamp01(Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab));
        Vector3 closest = bottom + t * ab;

        return Vector3.Distance(point, closest) < _bodyRadius;
    }
}

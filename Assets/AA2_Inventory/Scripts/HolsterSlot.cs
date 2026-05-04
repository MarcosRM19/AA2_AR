using UnityEngine;

public enum HolsterType { Body, World }

public class HolsterSlot : MonoBehaviour
{
    [Header("HolsterType")]
    [SerializeField] private HolsterType _holsterType = HolsterType.World;

    [SerializeField] private Vector3 _weaponUpAxis;
    [SerializeField] private Vector3 _weaponRightAxis;
    [SerializeField] private bool _holsterBeingUsed;

    public void SetWeaponUpAxis(Vector3 weaponUpAxis)
    {
        _weaponUpAxis = weaponUpAxis;
    }

    public void SetWeaponRightAxis(Vector3 weaponRightAxis)
    {
        _weaponRightAxis = weaponRightAxis;
    }

    public void SetHolsterBeingUsed(bool occupied)
    {
        _holsterBeingUsed = occupied;
    }

    public void SetHolsterType(HolsterType holsterType)
    {
        _holsterType = holsterType;
    }

    public Vector3 GetWeapomUpAxis()
    {
        if (_holsterType == HolsterType.Body)
        {
            Vector3 result = transform.rotation * _weaponUpAxis;
            Debug.Log($"UpAxis local: {_weaponUpAxis} → mundo: {result}");
            return result;
        }
        return _weaponUpAxis;
    }

    public Vector3 GetWeapomRightAxis()
    {
        if (_holsterType == HolsterType.Body)
        {
            Vector3 result = transform.rotation * _weaponRightAxis;
            Debug.Log($"RightAxis local: {_weaponRightAxis} → mundo: {result}");
            return result;
        }
        return _weaponRightAxis;
    }

    public HolsterType GetHolsterType()
    {
        return _holsterType;
    }

    public bool GetHolsterIsBeingUsed()
    {
        return _holsterBeingUsed;
    }
}

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
        return _weaponUpAxis;
    }

    public Vector3 GetWeapomRightAxis()
    {
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

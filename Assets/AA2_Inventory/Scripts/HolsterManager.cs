using System.Collections.Generic;
using UnityEngine;

public class HolsterManager : MonoBehaviour
{
    public static HolsterManager Instance { get; private set; }

    private List<HolsterSlot> holsters = new();

    void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);
    }

    public void RegisterHolster(HolsterSlot slot)
    {
        holsters.Add(slot);
        Debug.Log($"Registered holster: {slot.GetHolsterType()} at {slot.transform.position}");
    }

    public void UnregisterHolster(HolsterSlot slot)
    {
        holsters.Remove(slot);
    }

    public List<HolsterSlot> GetAll() => holsters;
}

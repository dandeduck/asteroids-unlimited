using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private UnitSelector selector;
    private Dictionary<int, Unit> units;

    private void Awake()
    {
        selector = GetComponent<UnitSelector>();
        units = new Dictionary<int, Unit>();
    }

    //This code is temporary. It is to be used until proper enemy system is implemented
    private void Start()
    {
        foreach (Ship ship in Object.FindObjectsOfType<Ship>())
            if (ship.Manager().GetInstanceID() == GetInstanceID())
                units.Add(((Unit)ship).Id(), ship);
    }

    public List<Unit> GetUnits()
    {
        return units.Values.ToList();
    }

    public bool Contains(Unit unit)
    {
        return units.ContainsKey(unit.Id());
    }

    public void AddUnit(Unit unit)
    {
        if (!units.ContainsKey(unit.Id()))
            units.Add(unit.Id(), unit);
    }

    public void KillUnit(Unit unit)
    {
        selector.DeselectUnit(unit);
        unit.OnKill();
        units.Remove(unit.Id());

        Destroy(unit.Object(), Time.deltaTime);
    }
}

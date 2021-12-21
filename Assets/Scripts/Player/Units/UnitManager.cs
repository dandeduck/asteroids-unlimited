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

    private void Start()
    {
        foreach (Unit unit in Resources.FindObjectsOfTypeAll<Ship>())
            units.Add(unit.Id(), unit);
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

        Destroy(unit.Transform(), Time.deltaTime);
    }
}

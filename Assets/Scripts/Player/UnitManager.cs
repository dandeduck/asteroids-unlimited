using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Ship[] initialUnits;

    private UnitSelector selector;
    private Dictionary<int, Unit> units;

    private void Awake()
    {
        selector = GetComponent<UnitSelector>();
        units = new Dictionary<int, Unit>();

        foreach (Unit unit in initialUnits)
            units.Add(unit.Id(), unit);
    }

    public ReadOnlyCollection<Unit> GetUnits()
    {
        return units.Values.ToList().AsReadOnly();
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

    public void DamageUnit(Unit unit, float damage)
    {
        if (units.ContainsKey(unit.Id()))
            if (unit.OnDamageTaken(damage))
                KillUnit(unit);
    }

    private void KillUnit(Unit unit)
    {
        selector.DeselectUnit(unit);
        unit.OnKill();
        units.Remove(unit.Id());

        Destroy(unit.Transform(), Time.deltaTime);
    }
}

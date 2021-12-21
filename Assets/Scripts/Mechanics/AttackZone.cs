using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private Dictionary<int, Unit> units;
    private float radius;
    private UnitManager manager;

    private void Awake()
    {
        units = new Dictionary<int, Unit>();
        radius = GetComponent<SphereCollider>().radius;
    }

    private void Start()
    {
        manager = GetComponentInParent<Ship>().Manager();
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();

        if (unit != null)
            if (!manager.Contains(unit))
                units.Add(unit.Id(), unit);
    }

    private void OnTriggerExit(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();

        if (unit != null)
            if (units.ContainsKey(unit.Id()))
                units.Remove(unit.Id());
    }

    public Unit[] GetUnitsInside()
    {
        return units.Values.ToArray();
    }

    public bool IsOutside(Unit unit)
    {
        return !units.ContainsKey(unit.Id());
    }

    public float GetRadius()
    {
        return radius;
    }
}

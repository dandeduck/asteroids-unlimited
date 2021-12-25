using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private Dictionary<int, Ship> attackableShips;
    private float radius;
    private ShipManager manager;
    private Ship ship;

    private void Awake()
    {
        attackableShips = new Dictionary<int, Ship>();
        radius = GetComponent<SphereCollider>().radius;
        ship = GetComponentInParent<Ship>();
        manager = ship.Manager();
    }

    private void Update()
    {
        if (!ship.IsInCombat() && attackableShips.Count > 0)
        {
            List<Ship> units = attackableShips.Values.ToList();

            if (units.Count > 0)
            {
                ShipsUtil.SortShipsByDistance(units, transform.position);
                ship.Attack(units[0], true); // can be false also.... may depend on ship or something
            }
        }
    }

    private void OnDestroyed()
    {
        attackableShips.Remove
    }

    private void OnTriggerEnter(Collider other)
    {
        Ship unit = other.GetComponent<Ship>();

        if (unit != null)
        {
            if (!manager.Contains(unit))
            {
                attackableShips.Add(unit.Id(), unit);
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ship unit = other.GetComponent<Ship>();

        if (unit != null)
            if (attackableShips.ContainsKey(unit.Id()))
                attackableShips.Remove(unit.Id());
    }

    public Ship[] GetShipsInside()
    {
        return attackableShips.Values.ToArray();
    }

    public bool IsOutside(Ship unit)
    {
        return !attackableShips.ContainsKey(unit.Id());
    }

    public float GetRadius()
    {
        return radius;
    }
}

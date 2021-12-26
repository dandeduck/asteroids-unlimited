using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private Dictionary<int, Ship> attackableShips;
    private float radius;
    private Ship ship;
    private LayerMask nonUnits;

    private void Awake()
    {
        attackableShips = new Dictionary<int, Ship>();
        radius = GetComponent<SphereCollider>().radius;
        ship = GetComponentInParent<Ship>();
        nonUnits = LayerMask.GetMask("NonUnits");
    }

    private void Update()
    {
        if (!ship.IsInCombat() && attackableShips.Count > 0)
        {
            List<Ship> ships = attackableShips.Values.ToList();

            if (ships.Count > 0)
            {
                ShipsUtil.SortShipsByDistance(ships, transform.position);
                ship.Attack(ships[0], true); // can be false also.... may depend on ship or something
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Ship enemyShip = other.GetComponent<Ship>();

        if (enemyShip != null && ship.GetManager().GetInstanceID() != enemyShip.GetManager().GetInstanceID())
        {
            attackableShips.Add(enemyShip.GetInstanceID(), enemyShip);
            enemyShip.AddDeathListener(OnDestroyed);
        }
    }

    private void OnDestroyed(Ship ship)
    {
        attackableShips.Remove(ship.GetInstanceID());
    }

    private void OnTriggerExit(Collider other)
    {
        Ship ship = other.GetComponent<Ship>();

        if (ship != null && attackableShips.ContainsKey(ship.GetInstanceID()))
            attackableShips.Remove(ship.GetInstanceID());
    }

    public Ship[] GetShipsInside()
    {
        return attackableShips.Values.ToArray();
    }

    public bool IsOutside(Ship ship)
    {
        return IsOutside(ship, 0);
    }

    public bool IsOutside(Ship ship, float additionalSpace)
    {
        if (attackableShips.ContainsKey(ship.GetInstanceID()))
            return false;

        Vector3 distance = ship.transform.position - transform.position;

        if (distance.magnitude > radius)
            return true;

        return Physics.Raycast(transform.position, distance, radius, nonUnits, QueryTriggerInteraction.Ignore);
    }

    public float GetRadius()
    {
        return radius;
    }
}

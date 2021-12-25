using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private Dictionary<int, Ship> attackableShips;
    private float radius;
    private Ship ship;

    private void Awake()
    {
        attackableShips = new Dictionary<int, Ship>();
        radius = GetComponent<SphereCollider>().radius;
        ship = GetComponentInParent<Ship>();
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
        return !attackableShips.ContainsKey(ship.GetInstanceID());
    }

    public float GetRadius()
    {
        return radius;
    }
}

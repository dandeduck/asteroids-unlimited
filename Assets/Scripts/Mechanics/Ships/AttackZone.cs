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
    }

    public Ship GetClosestShip()
    {
        List<Ship> ships = attackableShips.Values.ToList();

        if (ships.Count > 0)
        {
            ShipsUtil.SortShipsByDistance(ships, transform.position);
            
            return ships[0];
        }

        return null;
    }

    public bool Contains(Ship ship)
    {
        return attackableShips.ContainsKey(ship.GetInstanceID());
    }

    public bool IsEmpty()
    {
        return attackableShips.Count == 0;
    }

    public float GetRadius()
    {
        if (radius == 0) // this needs to be fixed better
            return GetComponent<SphereCollider>().radius;
        return radius;
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
}

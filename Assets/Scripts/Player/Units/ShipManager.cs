using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    private ShipSelector selector;
    private Dictionary<int, Ship> ships;

    private void Awake()
    {
        selector = GetComponent<ShipSelector>();
        ships = new Dictionary<int, Ship>();
    }

    //This code is temporary. It is to be used until proper enemy system is implemented
    private void Start()
    {
        foreach (Ship ship in Object.FindObjectsOfType<Ship>())
            if (ship.GetManager().GetInstanceID() == GetInstanceID())
                OnNewShip(ship);
    }

    public List<Ship> GetShips()
    {
        return ships.Values.ToList();
    }

    public bool Contains(Ship ship)
    {
        return ships.ContainsKey(ship.GetInstanceID());
    }

    public void AddShip(Ship ship)
    {
        if (!ships.ContainsKey(ship.GetInstanceID()))
            OnNewShip(ship);
    }

    public LayerMask GetLayer()
    {
        return 1 << gameObject.layer;
    }

    private void OnNewShip(Ship ship)
    {
        ship.gameObject.layer = gameObject.layer;

        ships.Add(ship.GetInstanceID(), ship);
        ship.AddDeathListener(OnDestroyed);
    }

    private void OnDestroyed(Ship ship)
    {
        selector.DeselectShip(ship);
        ships.Remove(ship.GetInstanceID());

        Destroy(ship.gameObject, Time.deltaTime);
    }
}

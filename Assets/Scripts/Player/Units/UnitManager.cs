using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    private ShipSelector selector;
    private Dictionary<int, Ship> units;

    private void Awake()
    {
        selector = GetComponent<ShipSelector>();
        units = new Dictionary<int, Ship>();
    }

    //This code is temporary. It is to be used until proper enemy system is implemented
    private void Start()
    {
        foreach (Ship ship in Object.FindObjectsOfType<Ship>())
            if (ship.Manager().GetInstanceID() == GetInstanceID())
                units.Add(((Ship)ship).Id(), ship);
    }

    public List<Ship> GetShips()
    {
        return units.Values.ToList();
    }

    public bool Contains(Ship unit)
    {
        return units.ContainsKey(unit.Id());
    }

    public void AddShip(Ship unit)
    {
        if (!units.ContainsKey(unit.Id()))
            units.Add(unit.Id(), unit);
    }

    public void KillShip(Ship unit)
    {
        selector.DeselectShip(unit);
        units.Remove(unit.Id());

        Destroy(unit.Object(), Time.deltaTime);
    }
}

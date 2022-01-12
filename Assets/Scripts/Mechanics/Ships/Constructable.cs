using UnityEngine;

public abstract class Constructable : MonoBehaviour
{
    [SerializeField] private float cost;
    [SerializeField] private float constructionTime;
    [SerializeField] private int size;

    private Ship ship;

    private void Awake()
    {
        ship = GetComponent<Ship>();
    }

    public Ship InitializeShip(ShipManager manager)
    {
        manager.AddShip(ship);
        ship.SetManager(manager);

        SetupIdentifiers(manager.GetComponent<Identifiers>());

        return ship;
    }

    public float GetCost()
    {
        return cost;
    }

    public float GetConstructionTime()
    {
        return constructionTime;
    }

    public int GetSize()
    {
        return size;
    }

    protected abstract void SetupIdentifiers(Identifiers identifiers);
}

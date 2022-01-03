using UnityEngine;

public class ShipBuilder : MonoBehaviour
{
    [SerializeField] private ShipManager shipManager;
    [SerializeField] private Ship frigateShip;

    private ShipHangar hangar;

    private void Awake()
    {
        hangar = GetComponent<ShipHangar>();
        frigateShip.SetManager(shipManager);
    }

    //This code is temporary
    private void Update()
    {
        BuildFrigate();
    }

    public bool BuildFrigate()
    {
        return hangar.BuyShip(frigateShip);
    }
}

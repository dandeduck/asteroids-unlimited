using UnityEngine;

public class ShipBuilder : MonoBehaviour
{
    [SerializeField] private ShipManager shipManager;
    [SerializeField] private Constructable frigateShip;

    private ShipHangar hangar;

    private void Awake()
    {
        hangar = GetComponent<ShipHangar>();
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

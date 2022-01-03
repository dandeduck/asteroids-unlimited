using UnityEngine;

public class ShipBuilder : MonoBehaviour
{
    [SerializeField] private ShipManager shipManager;
    [SerializeField] private Ship frigateShip;

    private ShipHangar hangar;
    private bool dick;

    private void Awake()
    {
        hangar = GetComponent<ShipHangar>();
        frigateShip.SetManager(shipManager);
        dick = false;
    }

    //This code is temporary
    private void Update()
    {
        if (!dick)
        {
            if (BuildFrigate())
                Debug.Log("Building frigate");
            dick = true;
        }
    }

    public bool BuildFrigate()
    {
        return hangar.BuyShip(frigateShip);
    }
}

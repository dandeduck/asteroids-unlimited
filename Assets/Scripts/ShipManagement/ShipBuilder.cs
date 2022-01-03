using UnityEngine;

public class ShipBuilder : MonoBehaviour
{
    [SerializeField] private Ship frigateShip;

    private ShipHangar hangar;

    private void Awake()
    {
        hangar = GetComponent<ShipHangar>();
    }

    public void BuildFrigate()
    {

    }
}

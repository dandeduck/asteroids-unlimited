using System.Collections.Generic;
using UnityEngine;

public class Captureable : MonoBehaviour
{
    [SerializeField] private float earn;
    [SerializeField] private float captureDuration;

    public float progress {get;}

    private List<Ship> capturingShips;
    private ShipManager capturer;

    private void Awake()
    {
        capturingShips = new List<Ship>();
    }

    private void Update()
    {
        if (capturer != null)
        {
            
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Ship ship = collider.GetComponent<Ship>();

        if (ship != null)
            OnShipEnter(ship);
    }

    private void OnShipEnter(Ship ship)
    {
        if (!IsBeingCaptured())
            CheckCapture();

        capturingShips.Add(ship);
        ship.AddDeathListener(OnDestroyed);
    }

    private bool IsBeingCaptured()
    {
        return capturer != null;
    }

    private void CheckCapture()
    {
        LayerMask firstShipLayer = capturingShips[0].gameObject.layer;

        foreach (Ship ship in capturingShips)
        {
            if (ship.gameObject.layer != firstShipLayer)
            {
                capturer = null;
                return;
            }
        }

        capturer = capturingShips[0].GetManager();
    }

    private void OnDestroyed(Ship ship)
    {
        capturingShips.Remove(ship);
        CheckCapture();
    }
}

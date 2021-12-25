using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float earn;
    [SerializeField] private float captureDuration;

    private List<Ship> capturingShips;
    private ShipManager capturer;

    private void OnTriggerEnter(Collider collider)
    {
        Ship unit = collider.GetComponent<Ship>();

        if (unit != null)
            OnShipEnter(unit);
    }

    private void OnShipEnter(Ship unit)
    {
        if (IsBeingCaptured())
        capturingShips.Add(unit);
    }

    private bool IsBeingCaptured()
    {
        return capturer != null;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Captureable : MonoBehaviour
{
    [SerializeField] private float captureDuration;

    private Dictionary<ShipManager, float> captureProgress;
    private List<Ship> capturingShips;
    private ShipManager capturer;
    private ShipManager holder;

    private void Awake()
    {
        captureProgress = new Dictionary<ShipManager, float>();
        capturingShips = new List<Ship>();
    }

    private void Start()
    {
        foreach (ShipManager manager in Object.FindObjectsOfType<ShipManager>())
            captureProgress.Add(manager, 0);
    }

    private void Update()
    {
        if (capturer != null)
        {
            foreach (ShipManager manager in captureProgress.Keys.ToList())
            {
                float progress = captureProgress[manager];
                if (manager != capturer)
                    captureProgress[manager] = Mathf.Max(0, progress - Time.deltaTime);
                else
                    captureProgress[manager] = Mathf.Min(captureDuration, progress + Time.deltaTime);
            }

            if (captureProgress[capturer] == captureDuration)
            {
                holder = capturer;
                capturer = null;
            }
        }
    }

    public ShipManager GetHolder()
    {
        return holder;
    }

    public ShipManager GetCurrentCapturer()
    {
        return capturer;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Ship ship = collider.GetComponent<Ship>();

        if (ship != null)
            OnShipEnter(ship);
    }

    private void OnTriggerExit(Collider collider)
    {
        Ship ship = collider.GetComponent<Ship>();

        if (ship != null)
            OnShipExit(ship); 
    }

    private void OnShipEnter(Ship ship)
    {
        capturingShips.Add(ship);

        if (capturingShips.Count == 1)
            capturer = ship.GetManager();

        ship.AddDeathListener(OnDestroyed);
    }

    private void OnShipExit(Ship ship)
    {
        capturingShips.Remove(ship);
        
        if (!(capturer == null && holder != null))
            CheckCapture();

        ship.RemoveDeathListener(OnDestroyed);
    }

    private bool IsBeingCaptured()
    {
        return capturer != null;
    }

    private void OnDestroyed(Ship ship)
    {
        capturingShips.Remove(ship);
        CheckCapture();
    }

    private void CheckCapture()
    {
        if (capturingShips.Count == 0)
        {
            capturer = null;
            return;
        }
        
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
}

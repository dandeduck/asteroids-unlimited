using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipHangar : MonoBehaviour
{
    [SerializeField] private ShipManager shipManager;
    [SerializeField] private int queueSize;
    [SerializeField] private int capacity;
    [SerializeField] private Vector3 waypoint;

    private FinanceManager financeManager;
    private Queue<Ship> buildQueue;
    private Coroutine buildingRoutine;

    private void Awake()
    {
        buildQueue = new Queue<Ship>();
        financeManager = shipManager.GetFinanceManager();
    }

    public bool BuyShip(Ship ship)
    {
        if (buildQueue.Count >= queueSize)
            return false;
        
        if (!financeManager.Spend(ship.GetCost()))
            return false;

        StartConstruction(ship);

        return true;
    }

    private void StartConstruction(Ship ship)
    {
        buildQueue.Enqueue(ship);

        if (buildingRoutine == null)
            buildingRoutine = StartCoroutine(Construction());
    }

    public void StopConstruction(int index)
    {
        if (index < buildQueue.Count)
        {
            if (buildingRoutine != null)
                StopCoroutine(buildingRoutine);
            buildQueue = new Queue<Ship>(buildQueue.Where((ship, i) => i != index));
        }
    }

    private IEnumerator Construction()
    {
        while (buildQueue.Count > 0)
        {
            Ship ship = buildQueue.Peek();

            yield return new WaitForSeconds(ship.GetConstructionTime());

            buildQueue.Dequeue();
            ship = Instantiate(ship, transform.position, transform.rotation);
            ship.SetManager(shipManager);
            ship.Move(waypoint);

            yield return null;
        }

        buildingRoutine = null;
    }
}

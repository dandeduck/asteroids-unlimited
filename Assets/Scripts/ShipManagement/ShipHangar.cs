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
    private int currentMaxArmySize;
    private int currentSize;

    private void Awake()
    {
        buildQueue = new Queue<Ship>();
        currentMaxArmySize = 0;
    }

    private void Start()
    {
        financeManager = shipManager.GetFinanceManager();
    }

    public bool BuyShip(Ship ship)
    {
        if (buildQueue.Count >= queueSize)
            return false;
        
        if (currentMaxArmySize + ship.GetSize() > capacity)
            return false;

        if (!financeManager.Spend(ship.GetCost()))
            return false;

        StartConstruction(ship);

        return true;
    }

    private void StartConstruction(Ship ship)
    {
        buildQueue.Enqueue(ship);
        currentMaxArmySize += ship.GetSize();

        if (buildingRoutine == null)
            buildingRoutine = StartCoroutine(Construction());
    }

    public void StopConstruction(int index)
    {
        if (index < buildQueue.Count)
        {
            if (buildingRoutine != null)
            {
                StopCoroutine(buildingRoutine);
                buildingRoutine = null;
            }

            buildQueue = new Queue<Ship>(buildQueue.Where((ship, i) => 
            {
                if (i != index)
                    return true;
                
                currentMaxArmySize -= ship.GetSize();
                return false;
            }));
        }
    }

    private IEnumerator Construction()
    {
        while (buildQueue.Count > 0)
        {
            Ship ship = buildQueue.Peek();

            yield return new WaitForSeconds(ship.GetConstructionTime());

            buildQueue.Dequeue();
            currentSize += ship.GetSize();
            ship = Instantiate(ship, transform.position, transform.rotation, transform.parent.parent); // scene as parent

            shipManager.AddShip(ship);
            ship.AddDeathListener(OnShipDeath);
            ship.Move(waypoint);

            yield return null;
        }

        buildingRoutine = null;
    }

    private void OnShipDeath(Ship killed)
    {
        int size = killed.GetSize();
        currentSize -= size;
        currentMaxArmySize -= size;
    }
}

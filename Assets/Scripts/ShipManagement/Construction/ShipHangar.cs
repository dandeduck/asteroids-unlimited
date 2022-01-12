using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipHangar : MonoBehaviour
{
    [SerializeField] private int queueSize;
    [SerializeField] private int capacity;
    [SerializeField] private Vector3 waypoint;
    [SerializeField] private ShipManager shipManager;

    private FinanceManager financeManager;
    private Queue<Constructable> buildQueue;
    private Coroutine buildingRoutine;
    private int currentMaxArmySize;
    private int currentSize;

    private void Awake()
    {
        buildQueue = new Queue<Constructable>();
        currentMaxArmySize = 0;
    }

    private void Start()
    {
        financeManager = shipManager.GetComponent<FinanceManager>();
    }

    public bool BuyShip(Constructable constructable)
    {
        if (buildQueue.Count >= queueSize)
            return false;
        
        if (currentMaxArmySize + constructable.GetSize() > capacity)
            return false;

        if (!financeManager.Spend(constructable.GetCost()))
            return false;

        StartConstruction(constructable);

        return true;
    }

    private void StartConstruction(Constructable constructable)
    {
        buildQueue.Enqueue(constructable);
        currentMaxArmySize += constructable.GetSize();

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

            buildQueue = new Queue<Constructable>(buildQueue.Where((ship, i) => 
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
            Constructable constructable = buildQueue.Peek();

            yield return new WaitForSeconds(constructable.GetConstructionTime());

            buildQueue.Dequeue();
            currentSize += constructable.GetSize();
            constructable = Instantiate(constructable, transform.position, transform.rotation, transform.parent.parent); // scene as parent

            Ship ship = constructable.InitializeShip(shipManager);
            ship.AddDeathListener(OnShipDeath);
            ship.Move(waypoint);

            yield return null;
        }

        buildingRoutine = null;
    }

    private void OnShipDeath(Ship killed)
    {
        int size = killed.GetComponent<Constructable>().GetSize();
        currentSize -= size;
        currentMaxArmySize -= size;
    }
}

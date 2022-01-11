using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Ship : MonoBehaviour
{
    private const float ANGLE_THRESHOLD = 5f;

    [SerializeField] private ShipManager manager;
    [SerializeField] private float health;
    [SerializeField] private float combatTurnSpeed;

    [SerializeField] private float cost;
    [SerializeField] private float constructionTime;
    [SerializeField] private int size;

    private NavMeshAgent agent;
    private CombatController combatController;
    private UnityEvent<Ship> death;

    private bool isMoving;
    private float acceleration;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        combatController = GetComponent<CombatController>();
        acceleration = agent.acceleration;

        death = new UnityEvent<Ship>();
    }

    private void Update()
    {
        if (HasReachedDestination() && isMoving)
        {
            isMoving = false;
            agent.isStopped = true;
        }
    }

    private bool HasReachedDestination()
    {
        return agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance;
    }

    public bool RotateTowards(Ship target)
    {
        Vector3 targetAdjusted = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion wantedRotation = Quaternion.LookRotation(targetAdjusted, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * combatTurnSpeed);

        return Mathf.Abs(wantedRotation.eulerAngles.y - transform.rotation.eulerAngles.y) <= ANGLE_THRESHOLD;
    }

    public void OnDeselect()
    {
    }

    public void OnSelect()
    {
    }

    public void Move(Ship ship)
    {
        Move(ship.transform.position);
    }

    public void Move(Vector3 position)
    {
        Move(position, 0, 0);
    }

    public void Move(Vector3 position, int arrivalIndex, int arrivalAmount)
    {
        agent.acceleration = acceleration;
        isMoving = true;
        agent.stoppingDistance = 0.1f + agent.radius * Mathf.Sqrt(arrivalAmount * 2) + Mathf.CeilToInt(arrivalIndex/2) * agent.radius;
        agent.isStopped = false;

        agent.SetDestination(position);
    }

    public void Attack(Ship ship)
    {
        combatController.ManualAttack(ship);
    }

    public void StopCombat()
    {
        combatController.StopCombat();
    }

    public void StopMovement()
    {
        if (IsAlive())
        {
            agent.acceleration = acceleration * 3;
            isMoving = false;
            agent.isStopped = true;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            OnDeath();
    }

    
    private void OnDeath()
    {
        death.Invoke(this);
        death.RemoveAllListeners();
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void AddDeathListener(UnityAction<Ship> action)
    {
        death.AddListener(action);
    }

    public void RemoveDeathListener(UnityAction<Ship> action)
    {
        death.RemoveListener(action);
    }

    public ShipManager GetManager()
    {
        return manager;
    }

    public void SetManager(ShipManager manager)
    {
        this.manager = manager;
    }

    public float GetCost()
    {
        return cost;
    }

    public float GetConstructionTime()
    {
        return constructionTime;
    }

    public int GetSize()
    {
        return size;
    }

    public Vector3 GetDestination()
    {
        return agent.destination;
    }
}

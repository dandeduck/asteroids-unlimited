using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Ship : MonoBehaviour
{
    private const float MIN_TURN_ANGLE = 5f;

    [SerializeField] private ShipManager manager;
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private float fireRateSeconds;
    [SerializeField] private float combatTurnSpeed;

    private NavMeshAgent agent;
    private AttackZone attackZone;
    private float radius;
    private IEnumerator combat;
    private bool inCombat;
    private bool inChase;
    private Ship target;
    private UnityEvent<Ship> death;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attackZone = GetComponentInChildren<AttackZone>();
        radius = agent.radius;

        inCombat = false;
        inChase = false;

        death = new UnityEvent<Ship>();
    }

    private void Update()
    {
        if (target != null && target.IsAlive() && inCombat && !inChase)
            LookAtTarget();
    }

    private void LookAtTarget()
    {
        Vector3 targetAdjusted = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion wantedRotation = Quaternion.LookRotation(targetAdjusted, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * combatTurnSpeed);
    }

    public void OnDeselect()
    {
        GetComponentInChildren<Renderer>().material.color = Color.black;
    }

    public void OnSelect()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
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
        agent.stoppingDistance = radius * Mathf.Sqrt(arrivalAmount * 2) + Mathf.CeilToInt(arrivalIndex/2) * radius;
        agent.isStopped = false;

        agent.SetDestination(position);
    }

    public void Attack(Ship ship, bool chase)
    {   
        Stop();

        combat = Combat(ship, chase);
        StartCoroutine(combat);

        Debug.Log("gonna mess up this dude " + ship);
    }

    public void Stop()
    {
        StopCombat();

        agent.isStopped = true;
    }

    public void StopCombat()
    {
        if (combat != null)
        {
            StopCoroutine(combat);
            inCombat = false;
            inChase = false;
            target = null;
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

    public float GetDamagePerSecond()
    {
        return damage / fireRateSeconds;
    }

    public bool IsInCombat()
    {
        return inCombat;
    }

    private void OnAttack(Ship ship)
    {
        Debug.Log("attacking " + ship);
    }

    private IEnumerator Combat(Ship ship, bool shouldChase)
    {        
        inCombat = true;

        while (ship != null && ship.IsAlive())
        {
            if (attackZone.IsOutside(ship))
            {
                if (shouldChase)
                {
                    inChase = true;
                    yield return Chase(ship);
                }
                else
                {
                    inCombat = false;
                    yield break;
                }
            }
            else
            {
                inChase = false;

                if (target == null)
                {
                    inCombat = true;
                    yield return RotateTowards(ship);
                    target = ship;
                }

                if (ship != null && ship.IsAlive())
                {
                    OnAttack(ship);
                    ship.TakeDamage(damage);
                }
            }

            if (inChase)
            {
                inChase = false;
                yield return null;
            }
            else
                yield return new WaitForSeconds(1/fireRateSeconds);
        }

        inCombat = false;
        target = null;
        yield break;
    }

    private IEnumerator Chase(Ship ship)
    {
        Move(ship);

        while (ship != null && ship.IsAlive() && attackZone.IsOutside(ship))
        {
            Vector3 shipPos = ship.transform.position;

            if ((agent.destination - shipPos).magnitude > attackZone.GetRadius())
                Move(shipPos);

            yield return null;
        }

        agent.isStopped = true;
    }

    private IEnumerator RotateTowards(Ship ship)
    {
        Vector3 targetAdjusted = new Vector3(ship.transform.position.x, transform.position.y, ship.transform.position.z) - transform.position;
        Quaternion wantedRotation = Quaternion.LookRotation(targetAdjusted, Vector3.up);

        while (ship != null && ship.IsAlive() && Mathf.Abs(wantedRotation.eulerAngles.y - transform.rotation.eulerAngles.y) > MIN_TURN_ANGLE)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * combatTurnSpeed);
            yield return null;
        }
    }

    public bool IsMoving()
    {
        return !agent.isStopped;
    }

    public void AddDeathListener(UnityAction<Ship> action)
    {
        death.AddListener(action);
    }

    public ShipManager GetManager()
    {
        return manager;
    }
}

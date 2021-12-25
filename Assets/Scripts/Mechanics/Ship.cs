using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Ship : MonoBehaviour, Ship
{
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
    private UnityEvent death;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attackZone = GetComponentInChildren<AttackZone>();
        radius = agent.radius;

        inCombat = false;
        inChase = false;
    }

    private void Update()
    {
        if (target != null && target.IsAlive() && inCombat && !inChase)
            LookAtTarget();
    }

    private void LookAtTarget()
    {
        Vector3 targetAdjusted = new Vector3(target.Object().transform.position.x, transform.position.y, target.Object().transform.position.z) - transform.position;
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

    public void Move(Ship unit)
    {
        Move(unit.Object().transform.position);
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

    public void Attack(Ship unit, bool chase)
    {   
        Stop();

        combat = Combat(unit, chase);
        StartCoroutine(combat);
    }

    public void StopCombat()
    {
        if (combat != null)
            StopCoroutine(combat);
    }

    public void Stop()
    {
        StopCombat();

        agent.isStopped = true;
        inCombat = false;
        inChase = false;
    }

    public void OnKill()
    {
        death.Invoke();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            OnKill();
            manager.KillShip(this);
        }
    }

    public GameObject Object()
    {
        return gameObject;
    }

    public bool IsAlive()
    {
        return health > 0;
    }
    
    //This code is temporary. It is to be used until proper enemy system is implemented
    public ShipManager Manager()
    {
        return manager;
    }

    public float GetDamagePerSecond()
    {
        return damage / fireRateSeconds;
    }

    public bool IsInCombat()
    {
        return inCombat;
    }

    private void OnAttack(Ship unit)
    {
    }

    private IEnumerator Combat(Ship unit, bool shouldChase)
    {
        while (unit != null && unit.IsAlive())
        {
            if (attackZone.IsOutside(unit))
            {
                if (shouldChase)
                {
                    inChase = true;
                    yield return Chase(unit);
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

                if (!inCombat)
                {
                    inCombat = true;
                    yield return RotateTowards(unit);
                    target = unit;
                }

                if (unit != null && unit.IsAlive())
                {
                    OnAttack(unit);
                    unit.TakeDamage(damage);
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
        yield break;
    }

    private IEnumerator Chase(Ship unit)
    {
        Move(unit);

        while (unit != null && unit.IsAlive() && attackZone.IsOutside(unit))
        {
            Vector3 unitPos = unit.Object().transform.position;

            if ((agent.destination - unitPos).magnitude > attackZone.GetRadius())
                Move(unitPos);

            yield return null;
        }

        agent.isStopped = true;
    }

    private IEnumerator RotateTowards(Ship unit)
    {
        Vector3 targetAdjusted = new Vector3(unit.Object().transform.position.x, transform.position.y, unit.Object().transform.position.z) - transform.position;
        Quaternion wantedRotation = Quaternion.LookRotation(targetAdjusted, Vector3.up);

        while (unit != null && unit.IsAlive() && Mathf.Abs(wantedRotation.eulerAngles.y - transform.rotation.eulerAngles.y) > 3)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * combatTurnSpeed);
            yield return null;
        }
    }

    public bool IsMoving()
    {
        return !agent.isStopped;
    }

    public void AddDeathListener(UnityAction action)
    {
        death.AddListener(action);
    }
}

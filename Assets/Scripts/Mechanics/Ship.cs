using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Ship : MonoBehaviour, Unit
{
    [SerializeField] private UnitManager manager;
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private float fireRateSeconds;

    private NavMeshAgent agent;
    private AttackZone attackZone;
    private float radius;
    private IEnumerator combat;
    private bool inCombat;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attackZone = GetComponentInChildren<AttackZone>();
        radius = agent.radius;
    }

    public void OnDeselect()
    {
        GetComponentInChildren<Renderer>().material.color = Color.black;
    }

    public void OnSelect()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
    }

    public void Move(Unit unit)
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

    public void Attack(Unit unit, bool chase)
    {   
        Stop();

        combat = Combat(unit, chase);
        StartCoroutine(combat);
    }

    public void Stop()
    {
        if (combat != null)
            StopCoroutine(combat);

        agent.isStopped = true;
        inCombat = false;
    }

    public void OnKill()
    {
        Debug.Log(gameObject.name + " dead");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            manager.KillUnit(this);

        Debug.Log(name + " Took damage now at " + health);
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
    public UnitManager Manager()
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

    private void OnAttack(Unit unit)
    {
        Debug.Log(gameObject.name + " is attacking " + unit.Object().name);
    }

    private IEnumerator Combat(Unit unit, bool shouldChase)
    {
        bool isChasing = false;

        inCombat = true;

        while (unit.IsAlive())
        {
            if (attackZone.IsOutside(unit))
            {
                if (shouldChase)
                {
                    isChasing = true;
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
                OnAttack(unit);
                unit.TakeDamage(damage);
            }

            if (isChasing)
            {
                isChasing = false;
                yield return null;
            }
            else
                yield return new WaitForSeconds(1/fireRateSeconds);
        }

        inCombat = false;
        yield break;
    }

    private IEnumerator Chase(Unit unit)
    {
        Move(unit);

        while (attackZone.IsOutside(unit))
        {
            Vector3 unitPos = unit.Object().transform.position;

            if ((agent.destination - unitPos).magnitude > attackZone.GetRadius())
                Move(unitPos);
            yield return null;
        }

        agent.isStopped = true;
    }

    public bool IsMoving()
    {
        return !agent.isStopped;
    }
}

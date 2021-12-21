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
        attackZone = GetComponent<AttackZone>();
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

    public void Move(Vector3 position, int arrivalIndex, int arrivalAmount)
    {
        agent.stoppingDistance = radius * Mathf.Sqrt(arrivalAmount * 2) + Mathf.CeilToInt(arrivalIndex/2) * radius;
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
        agent.isStopped = true;
        StopCoroutine(combat);

        inCombat = false;
    }

    public void OnKill()
    {
        Debug.Log(gameObject.name + " dead");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (damage < 0)
            manager.KillUnit(this);
    }

    public Transform Transform()
    {
        return transform;
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

    private void OnAttack(Unit unit)
    {
        if (!IsMoving())
            Debug.Log(gameObject.name + " is attacking " + unit.Transform().name);
    }

    private IEnumerator Combat(Unit unit, bool chase)
    {
        inCombat = true;

        if (!unit.IsAlive())
            EndCombat();

        if (attackZone.IsOutside(unit))
        {
            if (chase)
                Chase(unit);
            else
                EndCombat();
        }
        else
            unit.TakeDamage(damage);

        yield return new WaitForSeconds(fireRateSeconds);
    }

    private void Chase(Unit unit)
    {
        unit.Move(unit.Transform().position);

        while (!attackZone.IsOutside(unit))
        {
            Vector3 unitPos = unit.Transform().position;

            if ((agent.destination - unitPos).magnitude > attackZone.GetRadius())
                unit.Move(unitPos);
        }

        agent.isStopped = true;
    }

    private IEnumerator EndCombat()
    {
        inCombat = false;
        yield break;
    }

    private bool IsMoving()
    {
        return !agent.isStopped;
    }
}

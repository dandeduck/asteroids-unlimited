using UnityEngine;
using UnityEngine.AI;

public class Ship : MonoBehaviour, Unit
{
    private NavMeshAgent agent;
    private float radius;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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

    public void OnMove(Vector3 position, int arrivalIndex)
    {
        agent.stoppingDistance = radius * 4 + Mathf.CeilToInt(arrivalIndex/2) * radius;
        agent.SetDestination(position);
    }

    public void OnAttack(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public void OnKill()
    {
        throw new System.NotImplementedException();
    }

    public bool OnDamageTaken(float damage)
    {
        throw new System.NotImplementedException();
    }

    public Transform Transform()
    {
        return transform;
    }
}

using UnityEngine;
using UnityEngine.AI;

public class Ship : MonoBehaviour, Unit
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void OnDeselect()
    {
        GetComponentInChildren<Renderer>().material.color = Color.black;
    }

    public void OnSelect()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
    }

    public void OnAttack(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public void OnMove(Vector3 position)
    {
        agent.SetDestination(position);
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

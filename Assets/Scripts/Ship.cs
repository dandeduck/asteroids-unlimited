using UnityEngine;

[System.Serializable]
public class Ship : MonoBehaviour, Unit
{
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

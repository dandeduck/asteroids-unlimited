using UnityEngine;

public class Ship : MonoBehaviour, Unit
{
    public void Deselect()
    {
        GetComponentInChildren<Renderer>().material.color = Color.black;
    }

    public int id()
    {
        return GetInstanceID();
    }

    public void Kill()
    {
        throw new System.NotImplementedException();
    }

    public void OnAction()
    {
        throw new System.NotImplementedException();
    }

    public void Select()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
    }
}

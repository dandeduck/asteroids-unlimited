using UnityEngine;

public class Ship : MonoBehaviour, Unit
{
    public void Deselect()
    {
        Debug.Log("Deselected " + gameObject.name);
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
        Debug.Log("Selected " + gameObject.name);
    }
}

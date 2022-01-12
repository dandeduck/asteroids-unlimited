using UnityEngine;

public class Identifiers : MonoBehaviour
{
    [SerializeField] Material shipMaterial;

    public Material GetShipMaterial()
    {
        return shipMaterial;
    }
}

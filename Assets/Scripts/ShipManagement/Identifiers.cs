using UnityEngine;

public class Identifiers : MonoBehaviour
{
    [SerializeField] Material shipMaterial;
    [SerializeField] Material emissiveMaterial;
    [SerializeField] Material laserMaterial;

    public Material GetShipMaterial()
    {
        return shipMaterial;
    }

    public Material GetEmissiveMaterial()
    {
        return emissiveMaterial;
    }

    public Material GetLaserMaterial()
    {
        return laserMaterial;
    }
}

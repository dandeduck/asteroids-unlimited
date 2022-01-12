using UnityEngine;

public class CruiserConstructable : Constructable
{
    protected override void SetupIdentifiers(Identifiers identifiers)
    {
        Renderer shipRenderer = GetComponentInChildren<Renderer>();
        Material[] materials = shipRenderer.materials;

        materials[0] = identifiers.GetShipMaterial();
        materials[1] = identifiers.GetEmissiveMaterial();
        
        shipRenderer.materials = materials;

        GetComponentInChildren<LaserCannonArray>().SetLaserMaterial(identifiers.GetLaserMaterial());
    }
}

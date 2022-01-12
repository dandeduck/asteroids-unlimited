using UnityEngine;

public class CruiserConstructable : Constructable
{
    protected override void SetupIdentifiers(Identifiers identifiers)
    {
        GetComponentInChildren<Renderer>().material = identifiers.GetShipMaterial();
    }
}

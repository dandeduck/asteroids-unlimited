using System.Collections.Generic;
using UnityEngine;

public static class ShipsUtil
{
    private static LayerMask nonUnits = LayerMask.GetMask("NonUnits");

    public static void SortShipsByDistance(List<Ship> ships, Vector3 relativePoint)
    {
        ships.Sort((first, second) => 
            (first.transform.position - relativePoint).magnitude.CompareTo(
                (second.transform.position - relativePoint).magnitude));
    }

    public static bool HasObsticles(Transform source, Ship target)
    {
        return HasObsticles(source, target.transform.position - source.position);
    }

    public static bool HasObsticles(Transform source, Vector3 direction)
    {
        return Physics.Raycast(source.transform.position, direction, direction.magnitude, nonUnits, QueryTriggerInteraction.Ignore);
    }

    public static float Distance(Ship first, Ship second)
    {
        return (first.transform.position - second.transform.position).magnitude;
    }
}

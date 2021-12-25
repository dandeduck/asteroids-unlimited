using System.Collections.Generic;
using UnityEngine;

public static class ShipsUtil
{
    public static void SortShipsByDistance(List<Ship> units, Vector3 relativePoint)
    {
        units.Sort((first, second) => 
            (first.Object().transform.position - relativePoint).magnitude.CompareTo(
                (second.Object().transform.position - relativePoint).magnitude));
    }
}

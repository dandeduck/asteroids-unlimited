using System.Collections.Generic;
using UnityEngine;

public static class ShipsUtil
{
    public static void SortShipsByDistance(List<Ship> ships, Vector3 relativePoint)
    {
        ships.Sort((first, second) => 
            (first.transform.position - relativePoint).magnitude.CompareTo(
                (second.transform.position - relativePoint).magnitude));
    }
}

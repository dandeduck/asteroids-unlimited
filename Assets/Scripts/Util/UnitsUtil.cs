using System.Collections.Generic;
using UnityEngine;

public static class UnitsUtil
{
    public static void SortUnitsByDistance(List<Unit> units, Vector3 relativePoint)
    {
        units.Sort((first, second) => 
            (first.Object().transform.position - relativePoint).magnitude.CompareTo(
                (second.Object().transform.position - relativePoint).magnitude));
    }
}

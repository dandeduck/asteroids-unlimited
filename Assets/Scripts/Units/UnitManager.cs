using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    List<Unit> selectedUnits;

    private void Awake()
    {
        selectedUnits = new List<Unit>();
    }

    private void Update()
    {
        // Physics.OverlapBox();
    }
}

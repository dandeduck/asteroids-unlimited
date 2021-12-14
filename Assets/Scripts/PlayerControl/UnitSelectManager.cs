using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSelectManager : MonoBehaviour
{
    private List<Unit> selectedUnits;

    private void Awake()
    {
        selectedUnits = new List<Unit>();
    }

    private void Update()
    {
        // Physics.OverlapBox();

        if (Input.GetMouseButtonDown(0))
            OnLeftMouseClick();
    }

    public List<Unit> GetSelectedUnits()
    {
        return selectedUnits;
    }

    public void DeselectAll()
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.Deselect();
        }

        selectedUnits.Clear();
    }

    public void DeselectUnits(List<Unit> units)
    {
        selectedUnits = selectedUnits.Except(units).ToList();

        foreach (Unit unit in units)
        {
            unit.Deselect();
        }
    }

    private void OnLeftMouseClick()
    {
        Unit selected = MouseSelectedUnit();

        if (selected != null)
            OnUnitSelected(selected);
        else
            DeselectAll();
    }

    private Unit MouseSelectedUnit()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        Physics.Raycast(cameraRay, out hit, Camera.main.farClipPlane, LayerMask.GetMask("Units"));

        Collider collider = hit.collider;

        if (collider != null)
            return hit.collider.GetComponent<Unit>();
        
        return null;
    }

    private void OnUnitSelected(Unit unit)
    {
        if (Input.GetKey(KeyCode.LeftControl))
            SelectUnit(unit);
        else
            ReplaceSelection(unit);
    }

    private void ReplaceSelection(Unit unit)
    {
        if (unit != null  && !(selectedUnits.Count == 1 && unit.Equals(selectedUnits[0])))
        {
            DeselectAll();
            SelectUnit(unit);
        }
    }

    private void SelectUnit(Unit unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            unit.Select();
        }
    }

    private void ReplaceSelection(List<Unit> units)
    {
        DeselectAll();

        foreach (Unit unit in units)
        {
            SelectUnit(unit);
        }
    }
}

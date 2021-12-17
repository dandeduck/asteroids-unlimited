using System.Collections.Generic;
using UnityEngine;

public class UnitSelectManager : MonoBehaviour
{
    private const int DRAG_SELECT_HEIGHT = 5;

    [SerializeField] private RectTransform selectionBox;

    private Dictionary<int, Unit> selectedUnits;
    private LayerMask unitLayer;
    private bool dragSelect;
    private Vector2 dragStart;

    private void Awake()
    {
        selectedUnits = new Dictionary<int, Unit>();
        unitLayer = LayerMask.GetMask("Units");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClickSelect();
            dragStart = Input.mousePosition;
        }
        
        if (Input.GetMouseButton(0))
        {
            dragSelect = true;
            UpdateSelectionBox();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            selectionBox.sizeDelta = new Vector2(0, 0);
            dragSelect = false;

            OnDragSelect();
        }
    }

    private void UpdateSelectionBox()
    {
        Vector2 mousePos = Input.mousePosition;

        selectionBox.sizeDelta = VectorUtil.Abs(mousePos - dragStart);
        selectionBox.anchoredPosition = dragStart + (mousePos - dragStart) / 2;
    }

    private void OnDragSelect()
    {
        Vector2 screenCenter = selectionBox.anchoredPosition;
        Vector2 screenCorner = selectionBox.anchoredPosition + selectionBox.sizeDelta / 2;

        Vector3 worldCenter = VectorUtil.ScreenPosToGround(screenCenter);
        Vector3 worldCorner = VectorUtil.ScreenPosToGround(screenCorner);

        Vector3 halfExtents = worldCorner - worldCenter;
        halfExtents.y = DRAG_SELECT_HEIGHT;

        Collider[] colliders = Physics.OverlapBox(worldCenter, halfExtents, Camera.main.transform.rotation);

        foreach (Collider collider in colliders)
        {
            collider.GetComponent<Unit>().Select();    
        }
    }

    private void OnClickSelect()
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

        Physics.Raycast(cameraRay, out hit, Camera.main.farClipPlane, unitLayer);

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
        if (unit != null  && !(selectedUnits.Count == 1 && selectedUnits.ContainsKey(unit.id())))
        {
            DeselectAll();
            SelectUnit(unit);
        }
    }

    private void SelectUnit(Unit unit)
    {
        if (!selectedUnits.ContainsKey(unit.id()))
        {
            selectedUnits.Add(unit.id(), unit);
            unit.Select();
        }
    }

    private void SelectUnits(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            if (!selectedUnits.ContainsKey(unit.id()))
            {
                selectedUnits.Add(unit.id(), unit);
                unit.Select();
            }
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

    public Dictionary<int, Unit> GetSelectedUnits()
    {
        return selectedUnits;
    }

    public void DeselectAll()
    {
        foreach (Unit unit in selectedUnits.Values)
        {
            unit.Deselect();
        }

        selectedUnits.Clear();
    }

    public void DeselectUnits(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            if (selectedUnits.ContainsKey(unit.id()))
            {
                selectedUnits.Remove(unit.id());
                unit.Deselect();
            }
        }
    }

}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    private const int DRAG_SELECT_THRESHOLD = 40;

    [SerializeField] private RectTransform selectionBox;

    private Dictionary<int, Unit> selectedUnits;
    private LayerMask unitLayer;
    private bool dragSelect;
    private Vector2 dragStart;

    private UnitManager unitManager;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        unitManager = GetComponent<UnitManager>();

        selectedUnits = new Dictionary<int, Unit>();
        unitLayer = LayerMask.GetMask("Units");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            dragStart = Input.mousePosition;
        
        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox();

            if (selectionBox.sizeDelta.magnitude > DRAG_SELECT_THRESHOLD) //mouse is not really held
                dragSelect = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            if (!ChainSelect())
                DeselectAll();

            if (!dragSelect)
                OnClickSelect();
            else
            {
                OnDragSelect();

                selectionBox.sizeDelta = new Vector2(0, 0);
                dragSelect = false;
            }
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
        Vector2 min = selectionBox.anchoredPosition - selectionBox.sizeDelta / 2;
        Vector2 max = selectionBox.anchoredPosition + selectionBox.sizeDelta / 2;
            
        SelectUnitsBasedOnScreenPosition(min, max);
    }

    private void SelectUnitsBasedOnScreenPosition(Vector2 min, Vector2 max)
    {
        foreach (Unit unit in unitManager.GetUnits())
        {
            Vector2 screen = cam.WorldToScreenPoint(unit.Transform().position);

            if (VectorUtil.IsInsideRect(screen, min, max))
                SelectUnit(unit);
        }
    }

    private void OnClickSelect()
    {
        Unit selected = MouseSelectedUnit();

        if (selected != null)
            OnUnitSelected(selected);
        else if (!ChainSelect())
            DeselectAll();
    }

    private Unit MouseSelectedUnit()
    {
        Collider collider = VectorUtil.MousePosRaycast(cam, unitLayer);

        if (collider != null)
            return collider.GetComponent<Unit>();
        
        return null;
    }

    private void OnUnitSelected(Unit unit)
    {
        if (unitManager.Contains(unit))
            SelectUnit(unit);
    }

    private bool ChainSelect()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }

    private void SelectUnits(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            if (!selectedUnits.ContainsKey(unit.Id()))
            {
                selectedUnits.Add(unit.Id(), unit);
                unit.OnSelect();
            }
        }
    }

    private void ReplaceSelection(List<Unit> units)
    {
        DeselectAll();

        foreach (Unit unit in units)
            SelectUnit(unit);
    }

    private void SelectUnit(Unit unit)
    {
        if (!selectedUnits.ContainsKey(unit.Id()))
        {
            selectedUnits.Add(unit.Id(), unit);
            unit.OnSelect();
        }
    }

    public ReadOnlyCollection<Unit> GetSelectedUnits()
    {
        return selectedUnits.Values.ToList().AsReadOnly();
    }

    public void DeselectAll()
    {
        foreach (Unit unit in selectedUnits.Values)
            unit.OnDeselect();

        selectedUnits.Clear();
    }

    public void DeselectUnits(List<Unit> units)
    {
        foreach (Unit unit in units)
            DeselectUnit(unit);   
    }

    public void DeselectUnit(Unit unit)
    {
        if (selectedUnits.ContainsKey(unit.Id()))
        {
            selectedUnits.Remove(unit.Id());
            unit.OnDeselect();
        }
    }
}

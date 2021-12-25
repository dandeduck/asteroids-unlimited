using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
    private const int DRAG_SELECT_THRESHOLD = 40;

    [SerializeField] private RectTransform selectionBox;

    private Dictionary<int, Ship> selectedShips;
    private LayerMask unitLayer;
    private bool dragSelect;
    private Vector2 dragStart;

    private ShipManager unitManager;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        unitManager = GetComponent<ShipManager>();

        selectedShips = new Dictionary<int, Ship>();
        unitLayer = LayerMask.GetMask("Ships");
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
            
        SelectShipsBasedOnScreenPosition(min, max);
    }

    private void SelectShipsBasedOnScreenPosition(Vector2 min, Vector2 max)
    {
        foreach (Ship unit in unitManager.GetShips())
        {
            Vector2 screen = cam.WorldToScreenPoint(unit.Object().transform.position);

            if (VectorUtil.IsInsideRect(screen, min, max))
                SelectShip(unit);
        }
    }

    private void OnClickSelect()
    {
        Ship selected = MouseSelectedShip();

        if (selected != null)
            OnShipSelected(selected);
        else if (!ChainSelect())
            DeselectAll();
    }

    private Ship MouseSelectedShip()
    {
        Collider collider = VectorUtil.MousePosRaycast(cam, unitLayer);

        if (collider != null)
            return collider.GetComponent<Ship>();
        
        return null;
    }

    private void OnShipSelected(Ship unit)
    {
        if (unitManager.Contains(unit))
            SelectShip(unit);
    }

    private bool ChainSelect()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }

    private void SelectShips(List<Ship> units)
    {
        foreach (Ship unit in units)
        {
            if (!selectedShips.ContainsKey(unit.Id()))
            {
                selectedShips.Add(unit.Id(), unit);
                unit.OnSelect();
            }
        }
    }

    private void ReplaceSelection(List<Ship> units)
    {
        DeselectAll();

        foreach (Ship unit in units)
            SelectShip(unit);
    }

    private void SelectShip(Ship unit)
    {
        if (!selectedShips.ContainsKey(unit.Id()))
        {
            selectedShips.Add(unit.Id(), unit);
            unit.OnSelect();
        }
    }

    public List<Ship> GetSelectedShips()
    {
        return selectedShips.Values.ToList();
    }

    public void DeselectAll()
    {
        foreach (Ship unit in selectedShips.Values)
            unit.OnDeselect();

        selectedShips.Clear();
    }

    public void DeselectShips(List<Ship> units)
    {
        foreach (Ship unit in units)
            DeselectShip(unit);   
    }

    public void DeselectShip(Ship unit)
    {
        if (selectedShips.ContainsKey(unit.Id()))
        {
            selectedShips.Remove(unit.Id());
            unit.OnDeselect();
        }
    }
}

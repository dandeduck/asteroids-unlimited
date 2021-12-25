using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
    private const int DRAG_SELECT_THRESHOLD = 3;

    [SerializeField] private RectTransform selectionBox;

    private Dictionary<int, Ship> selectedShips;
    private LayerMask shipLayer;
    private bool dragSelect;
    private Vector2 dragStart;

    private ShipManager shipManager;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        shipManager = GetComponent<ShipManager>();

        selectedShips = new Dictionary<int, Ship>();
        shipLayer = shipManager.GetLayer();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            dragStart = Input.mousePosition;
        
        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox();

            if (selectionBox.sizeDelta.magnitude > DRAG_SELECT_THRESHOLD)
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
        foreach (Ship ship in shipManager.GetShips())
        {
            Vector2 screen = cam.WorldToScreenPoint(ship.transform.position);

            if (VectorUtil.IsInsideRect(screen, min, max))
                SelectShip(ship);
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
        Collider collider = VectorUtil.MousePosRaycast(cam, shipLayer);

        if (collider != null)
            return collider.GetComponent<Ship>();
        
        return null;
    }

    private void OnShipSelected(Ship ship)
    {
        if (shipManager.Contains(ship))
            SelectShip(ship);
    }

    private bool ChainSelect()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }

    private void SelectShips(List<Ship> ships)
    {
        foreach (Ship ship in ships)
        {
            if (!selectedShips.ContainsKey(ship.GetInstanceID()))
            {
                selectedShips.Add(ship.GetInstanceID(), ship);
                ship.OnSelect();
            }
        }
    }

    private void ReplaceSelection(List<Ship> ships)
    {
        DeselectAll();

        foreach (Ship ship in ships)
            SelectShip(ship);
    }

    private void SelectShip(Ship ship)
    {
        if (!selectedShips.ContainsKey(ship.GetInstanceID()))
        {
            selectedShips.Add(ship.GetInstanceID(), ship);
            ship.OnSelect();
        }
    }

    public List<Ship> GetSelectedShips()
    {
        return selectedShips.Values.ToList();
    }

    public void DeselectAll()
    {
        foreach (Ship ship in selectedShips.Values)
            ship.OnDeselect();

        selectedShips.Clear();
    }

    public void DeselectShips(List<Ship> ships)
    {
        foreach (Ship ship in ships)
            DeselectShip(ship);   
    }

    public void DeselectShip(Ship ship)
    {
        if (selectedShips.ContainsKey(ship.GetInstanceID()))
        {
            selectedShips.Remove(ship.GetInstanceID());
            ship.OnDeselect();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ShipDelegator : MonoBehaviour
{
    private ShipSelector selector;
    private ShipManager manager;
    private Camera cam;
    private LayerMask shipMask;

    List<Ship> selected;

    private void Awake()
    {
        selector = GetComponent<ShipSelector>();
        manager = GetComponent<ShipManager>();
        cam = GetComponentInChildren<Camera>();
        shipMask = LayerMask.GetMask("Units");
    }

    private void LateUpdate()
    {   
        if (Input.GetMouseButtonDown(1))
            OnAction();
    }

    private void OnAction()
    {
        selected = selector.GetSelectedShips();
        Collider collider = VectorUtil.MousePosRaycast(cam, shipMask);

        if (collider != null)
            OnAttack(collider.GetComponent<Ship>());
        else
            OnMove(VectorUtil.MousePosToGround(cam));
    }

    private void OnAttack(Ship target)
    {
        if (target != null)
        {
            if (manager.Contains(target))
                OnMove(target.transform.position);
            else
                foreach (Ship ship in selected)
                    ship.Attack(target, true);
        }
    }

    private void OnMove(Vector3 position)
    {
        ShipsUtil.SortShipsByDistance(selected, position);
        
        for (int i = 0; i < selected.Count; i++)
        {
            selected[i].StopCombat();
            selected[i].Move(position, i, selected.Count);
        }
    }
}
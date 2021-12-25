using System.Collections.Generic;
using UnityEngine;

public class ShipDelegator : MonoBehaviour
{
    private ShipSelector selector;
    private ShipManager manager;
    private Camera cam;
    private LayerMask enemyShipMask;

    List<Ship> selected;

    private void Awake()
    {
        selector = GetComponent<ShipSelector>();
        manager = GetComponent<ShipManager>();
        cam = GetComponentInChildren<Camera>();
        enemyShipMask = ~manager.GetLayer();
    }

    private void LateUpdate()
    {   
        if (Input.GetMouseButtonDown(1))
            OnAction();
    }

    private void OnAction()
    {
        selected = selector.GetSelectedShips();
        Collider collider = VectorUtil.MousePosRaycast(cam, enemyShipMask);

        if (collider != null)
            OnAttack(collider.GetComponent<Ship>());
        else
            OnMove(VectorUtil.MousePosToGround(cam));
    }

    private void OnAttack(Ship target)
    {
        if (target != null)
            foreach (Ship ship in selected)
                ship.Attack(target, true);
        else
            OnMove(VectorUtil.MousePosToGround(cam));
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

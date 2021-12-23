using System.Collections.Generic;
using UnityEngine;

public class UnitDelegator : MonoBehaviour
{
    private UnitSelector selector;
    private UnitManager manager;
    private Camera cam;
    private LayerMask unitMask;

    List<Unit> selected;

    private void Awake()
    {
        selector = GetComponent<UnitSelector>();
        manager = GetComponent<UnitManager>();
        cam = GetComponentInChildren<Camera>();
        unitMask = LayerMask.GetMask("Units");
    }

    private void LateUpdate()
    {   
        if (Input.GetMouseButtonDown(1))
            OnAction();
    }

    private void OnAction()
    {
        selected = selector.GetSelectedUnits();
        Collider collider = VectorUtil.MousePosRaycast(cam, unitMask);

        if (collider != null)
            OnAttack(collider.GetComponent<Unit>());
        else
            OnMove(VectorUtil.MousePosToGround(cam));
    }

    private void OnAttack(Unit target)
    {
        if (target != null)
        {
            if (manager.Contains(target))
                OnMove(target.Object().transform.position);
            else
                foreach (Unit unit in selected)
                    unit.Attack(target, true);
        }
    }

    private void OnMove(Vector3 position)
    {
        UnitsUtil.SortUnitsByDistance(selected, position);
        
        for (int i = 0; i < selected.Count; i++)
        {
            selected[i].StopCombat();
            selected[i].Move(position, i, selected.Count);
        }
    }
}

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
                OnMove(target.Transform().position);
            else
                foreach (Unit unit in selected)
                    unit.OnAttack(target);
        }
    }

    private void OnMove(Vector3 position)
    {
        selected.Sort((first, second) => (first.Transform().position - position).magnitude.CompareTo((second.Transform().position - position).magnitude));
        
        for (int i = 0; i < selected.Count; i++)
            selected[i].OnMove(position, i, selected.Count);
    }
}

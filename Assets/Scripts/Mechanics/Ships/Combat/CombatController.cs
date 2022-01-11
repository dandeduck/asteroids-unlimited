using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private const float ANGLE_THRESHOLD = 5f;

    private Coroutine combat;

    private Ship ship;
    private Weapon[] weapons;
    private float minRange;
    private bool isInCombat;

    private void Awake()
    {
        ship = GetComponent<Ship>();
        weapons = GetComponentsInChildren<Weapon>();

        minRange = FindMinRange();
    }

    private void Update()
    {
        if (!isInCombat)
        {
            
        }
    }

    public void ManualAttack(Ship target)
    {
        ship.StopMovement();
        StopCombat();

        combat = StartCoroutine(Combat(target));
        Target(target);
    }

    public void StopCombat()
    {
        StopAllCoroutines();
        ship.StopMovement();
        StopShooting();
    }

    private void Target(Ship target)
    {
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].Target(target);
    }

    private void StopShooting()
    {
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].StopShooting();
    }

    private float FindMinRange()
    {
        float smallest = -1;

        for (int i = 0; i < weapons.Length; i++)
            if (smallest == -1 || weapons[i].GetRange() < smallest)
                smallest = weapons[i].GetRange();

        return smallest;   
    }

    private IEnumerator Combat(Ship target)
    {
        isInCombat = true;

        while (target != null && target.IsAlive())
        {
            if (!AllWeaponsInRange(target))
                ship.Move(target);

            else
            {
                ship.StopMovement();
                yield return RotateTowards(target);

            }

            yield return null;
        }

        ship.StopMovement();
        StopShooting();
    }

    private IEnumerator RotateTowards(Ship target)
    {
        do {
            yield return null;
        } while (target != null && !ship.RotateTowards(target));
    }

    private bool AllWeaponsInRange(Ship target)
    {
        for (int i = 0; i < weapons.Length; i++)
            if (!weapons[i].IsInRange(target))
                return false;

        return true;
    }
}

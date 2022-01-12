using System.Collections;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private const float ANGLE_THRESHOLD = 5f;

    private Ship ship;
    private Weapon[] weapons;
    private AttackZone attackZone;

    private bool isInCombat;
    private Coroutine combat;

    private void Awake()
    {
        ship = GetComponent<Ship>();
        weapons = GetComponentsInChildren<Weapon>();
        attackZone = GetComponentInChildren<AttackZone>();
    }

    private void Update()
    {
        if (!isInCombat && !ship.IsMoving() && !attackZone.IsEmpty())
            Attack(attackZone.GetClosestShip());//TODO: not attack if abstracted?
    }

    public void Attack(Ship target)
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
        isInCombat = false;
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
                ship.RotateTowards(target);
            }

            yield return null;
        }

        isInCombat = false;
        ship.StopMovement();
        StopShooting();
    }

    private bool AllWeaponsInRange(Ship target)
    {
        for (int i = 0; i < weapons.Length; i++)
            if (!weapons[i].IsInRange(target))
                return false;

        return true;
    }
}

using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private AttackZone attackZone;
    [SerializeField] private float range;
    [SerializeField] private float rateOfFire;

    private Coroutine shootingRoutine;
    private bool isShooting;

    private void Update()
    {
        if(!IsShooting() && !attackZone.IsEmpty())
        {
            Ship closest = attackZone.GetClosestShip();
            
            if(CanShootAt(closest))
                ShootAt(closest);
        }
    }

    public void ShootAt(Ship target)
    {
        StopShooting();
        StartShooting(target);
    }

    private void StartShooting(Ship target)
    {
        shootingRoutine = StartCoroutine(Shooting(target));

        OnShootingStart(target);
    }

    private void StopShooting()
    {
        isShooting = false;

        if(shootingRoutine != null)
            StopCoroutine(shootingRoutine);

        OnShootingStop();
    }

    public bool IsShooting()
    {
        return isShooting;
    }

    protected IEnumerator Shooting(Ship target)
    {
        isShooting = true;

        while (target != null)
        {
            if (CanShootAt(target) && IsInRange(target))
            {
                Shoot(target);
                yield return new WaitForSeconds(1 / rateOfFire);
            }

            else
                yield return null;
        }

        isShooting = false;
    }

    public float GetRange()
    {
        return range;
    }

    public float GetRateOfFire()
    {
        return rateOfFire;
    }

    public bool IsInRange(Ship target)
    {
        if (!attackZone.Contains(target))
            return false;

        Vector3 distance = target.transform.position - transform.position;

        if (distance.magnitude > range)
            return false;

        return !ShipsUtil.HasObsticles(transform, distance);
    }

    public abstract void Shoot(Ship target);
    public abstract bool CanShootAt(Ship target);

    protected virtual void OnShootingStart(Ship target) {}
    protected virtual void OnShootingStop() {}
}

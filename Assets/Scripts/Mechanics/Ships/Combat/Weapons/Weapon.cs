using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, Shooter
{
    [SerializeField] private AttackZone attackZone;
    [SerializeField] private float range;
    [SerializeField] private float rateOfFire;

    private Coroutine shootingRoutine;
    private bool isShooting;

    private void Update()
    {
        if(!isShooting && !attackZone.IsEmpty())
        {
            Ship closest = attackZone.GetClosestShip();
            
            if(closest != null && CanShootAt(closest))
                Target(closest);
        }
    }

    public void Target(Ship target)
    {
        StopShooting();
        StartShooting(target);
    }

    private void StartShooting(Ship target)
    {
        shootingRoutine = StartCoroutine(Shooting(target));

        OnShootingStart(target);
    }

    public void StopShooting()
    {
        isShooting = false;

        if(shootingRoutine != null)
            StopCoroutine(shootingRoutine);

        OnShootingStop();
    }

    protected IEnumerator Shooting(Ship target)
    {
        isShooting = true;

        while (target != null)
        {
            if (CanShootAt(target) && IsInRange(target))
            {
                yield return Shoot(target);
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

    public abstract bool CanShootAt(Ship target);
    public abstract IEnumerator Shoot(Ship target);

    protected virtual void OnShootingStart(Ship target) {}
    protected virtual void OnShootingStop() {}
}

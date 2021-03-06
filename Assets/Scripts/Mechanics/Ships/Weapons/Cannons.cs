using System.Collections;
using UnityEngine;

public class Cannons : WeaponSystem
{
    [SerializeField] private AttackZone attackZone;

    private float shootingDelay;
    private LaserCannon[] cannons;
    private Coroutine shootingStart;

    private void Awake()
    {
        cannons = GetComponentsInChildren<LaserCannon>();
    }

    private void Start() // Must have same lasers I guess
    {
        shootingDelay = attackZone.GetRadius() / cannons[0].GetComponentInChildren<Laser>().GetSpeed() / 2;
    }

    public override bool IsShooting()
    {
        return cannons[0].IsShooting();
    }

    public override void StartShooting(Ship target)
    {
        shootingStart = StartCoroutine(Shoot(target));
    }

    public override void StopShooting()
    {
        if (shootingStart != null)
            StopCoroutine(shootingStart);

        for (int i = 0; i < cannons.Length; i++)
            cannons[i].StopShooting();
    }

    private IEnumerator Shoot(Ship target)
    {
        for (int i = 0; i < cannons.Length; i++)
        {
            cannons[i].StartShooting(target);

            yield return new WaitForSeconds(shootingDelay);
        }
    }
}

using System.Collections;
using UnityEngine;

public class LaserCannonArray : DirectionalWeapon
{
    private LaserCannon[] cannons;
    private float shootingDelay;

    private void Start()
    {
        cannons = GetComponentsInChildren<LaserCannon>();
        shootingDelay = GetRange() / cannons[0].GetComponentInChildren<Laser>().GetSpeed() / 2;//ugh
    }

    public override IEnumerator Shoot(Ship target)
    {
        for (int i = 0; i < cannons.Length; i++)
        {
            yield return cannons[i].Shoot(target);
            yield return new WaitForSeconds(shootingDelay);
        }
    }
}

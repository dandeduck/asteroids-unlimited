using System.Collections;
using UnityEngine;

public class LaserCannonArray : DirectionalWeapon
{
    private LaserCannon[] cannons;
    private float shootingDelay;
    private int shootingCannon;

    private void Awake()
    {
        cannons = GetComponentsInChildren<LaserCannon>();
        shootingCannon = 0;
    }

    public void SetLaserMaterial(Material material)
    {
        for (int i = 0; i < cannons.Length; i++)
            cannons[i].SetLaserMaterial(material);
    }

    public override IEnumerator Shoot(Ship target)
    {
        yield return cannons[shootingCannon %= cannons.Length].Shoot(target);
        shootingCannon ++;
    }
}

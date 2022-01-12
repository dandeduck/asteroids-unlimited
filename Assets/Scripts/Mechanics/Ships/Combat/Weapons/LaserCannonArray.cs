using System.Collections;
using UnityEngine;

public class LaserCannonArray : DirectionalWeapon
{
    [SerializeField] Laser laser;

    private LaserCannon[] cannons;
    private float shootingDelay;

    private void Awake()
    {
        cannons = GetComponentsInChildren<LaserCannon>();
        shootingDelay = GetRange() / laser.GetSpeed();
    }

    public void SetLaserMaterial(Material material)
    {
        for (int i = 0; i < cannons.Length; i++)
            cannons[i].SetLaserMaterial(material);
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

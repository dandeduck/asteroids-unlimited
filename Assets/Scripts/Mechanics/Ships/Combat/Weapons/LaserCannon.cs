using System.Collections;
using UnityEngine;

public class LaserCannon : DirectionalWeapon
{
    private const float MAX_SHOOTING_OFFSET = 0.2f;

    [SerializeField] private Laser ammunition;

    private Laser[] lasers;
    private int shotCount;

    private void OnEnable()
    {        
        if (GetRateOfFire() != 0)
            CreateLasers();
    }

    public void SetLaserMaterial(Material material)
    {
        for (int i = 0; i < lasers.Length; i++)
            lasers[i].SetMaterial(material);
    }

    public override IEnumerator Shoot(Ship target)
    {
        lasers[shotCount%lasers.Length].Shoot(target);
        shotCount++;
        yield return new WaitForEndOfFrame();
    }

    private void CreateLasers()
    {
        int laserAmount = Mathf.CeilToInt(GetRange() / ammunition.GetSpeed() * GetRateOfFire());
        lasers = new Laser[laserAmount];

        for (int i = 0; i < laserAmount; i++)
        {
            lasers[i] = Instantiate(ammunition, transform.position, transform.rotation);
            lasers[i].transform.parent = transform;
        }
    }
}

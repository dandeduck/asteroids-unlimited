using System.Collections;
using UnityEngine;

public class LaserCannon : MonoBehaviour
{
    [SerializeField] private AttackZone attackZone;
    [SerializeField] private float rateOfFire;
    [SerializeField] private Laser ammunition;

    private Laser[] lasers;
    private Coroutine shooting;
    private int shotCount;
    private bool isShooting;

    private void OnEnable()
    {
        float maxShootingDistance = attackZone.GetRadius();
        
        if (rateOfFire != 0)
            CreateLasers(maxShootingDistance);
    }

    public bool IsShooting()
    {
        return isShooting;
    }

    public void StopShooting()
    {
        if (shooting != null)
            StopCoroutine(shooting);
        
        isShooting = false;
    }

    public void StartShooting(Ship target)
    {
        StopShooting();

        shooting = StartCoroutine(Shoot(target));
    }

    private IEnumerator Shoot(Ship target)
    {
        isShooting = true;

        while (target != null)
        {
            lasers[shotCount%lasers.Length].Shoot(target);
            shotCount++;

            yield return new WaitForSeconds(1/rateOfFire);
        }

        isShooting = false;
    }

    private void CreateLasers(float distance)
    {
        int laserAmount = Mathf.CeilToInt(distance / ammunition.GetSpeed() * rateOfFire);
        lasers = new Laser[laserAmount];

        for (int i = 0; i < laserAmount; i++)
        {
            lasers[i] = Instantiate(ammunition, transform.position, transform.rotation);
            lasers[i].transform.parent = transform;
        }
    }
}

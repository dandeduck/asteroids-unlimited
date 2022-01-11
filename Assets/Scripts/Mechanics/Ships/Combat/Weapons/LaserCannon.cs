using UnityEngine;

public class LaserCannon : Weapon
{
    private const float MAX_SHOOTING_OFFSET = 10f;

    [SerializeField] private Laser ammunition;

    private Laser[] lasers;
    private int shotCount;

    private void OnEnable()
    {        
        if (GetRateOfFire() != 0)
            CreateLasers();
    }

    protected override void Shoot(Ship target)
    {
        lasers[shotCount%lasers.Length].Shoot(target);
        shotCount++;
    }

    public override bool CanShootAt(Ship target)
    {
        Vector3 targetAdjusted = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion wantedRotation = Quaternion.LookRotation(targetAdjusted, Vector3.up);

        return (wantedRotation.eulerAngles - transform.rotation.eulerAngles).magnitude < MAX_SHOOTING_OFFSET;
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

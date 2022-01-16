using System.Collections;
using UnityEngine;

public class LaserCannon : MonoBehaviour, Shooter
{
    private const int LASER_AMOUNT = 10;

    [SerializeField] private Laser ammunition;

    private Laser[] lasers;
    private int shotCount;

    private void OnEnable()
    {
        CreateLasers();
    }

    public void SetLaserMaterial(Material material)
    {
        for (int i = 0; i < lasers.Length; i++)
            lasers[i].SetMaterial(material);
    }

    public IEnumerator Shoot(Ship target)
    {
        lasers[shotCount%=lasers.Length].Shoot(target);
        shotCount++;
        yield return new WaitForEndOfFrame();
    }

    private void CreateLasers() // will be turned to vfx or changed somehow to eliminate this
    {
        lasers = new Laser[LASER_AMOUNT];

        for (int i = 0; i < LASER_AMOUNT; i++)
        {
            lasers[i] = Instantiate(ammunition, transform.position, transform.rotation);
            lasers[i].transform.parent = transform;
        }
    }
}

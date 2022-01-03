using UnityEngine;

public abstract class WeaponSystem : MonoBehaviour
{
    public abstract void StartShooting(Ship target);
    public abstract void StopShooting();
    public abstract bool IsShooting();
}

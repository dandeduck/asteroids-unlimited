using UnityEngine;

public abstract class DirectionalWeapon : Weapon
{
    [SerializeField] private float maxShootingOffset = 0.2f;

    public override bool CanShootAt(Ship target)
    {
        Vector3 targetAdjusted = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion wantedRotation = Quaternion.LookRotation(targetAdjusted, Vector3.up);

        float closeAngle = Mathf.Abs((wantedRotation.eulerAngles - transform.rotation.eulerAngles).magnitude * Mathf.Deg2Rad);
        float distance = Mathf.Abs((transform.position - target.transform.position).magnitude);

        return closeAngle < 45f * Mathf.Deg2Rad && Mathf.Tan(closeAngle) * distance <= maxShootingOffset;
    }
}

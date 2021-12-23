using UnityEngine;

public interface Unit
{
    void OnSelect();
    void OnDeselect();

    void Move(Vector3 position, int arrivalIndex, int arrivalAmount);
    void Move(Vector3 position)
    {
        Move(position, 0, 0);
    }

    void Attack(Unit unit, bool chase);
    void OnKill();
    void TakeDamage(float damage);
    void StopCombat();
    bool IsAlive();

    GameObject Object();
    int Id()
    {
        return Object().GetInstanceID();
    }
}
